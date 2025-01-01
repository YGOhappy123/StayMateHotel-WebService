using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Response;
using server.Dtos.Service; // Đảm bảo tạo các DTO tương ứng cho Service
using server.Interfaces.Services; // Các dịch vụ liên quan đến Service
using server.Models;
using server.Utilities;
using server.Queries;
using server.Extensions.Mappers;

namespace server.Controllers
{
    [ApiController]
    [Route("/services")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        // Lấy tất cả các Service với phân trang hoặc lọc (BaseQueryObject)
        [HttpGet]
        public async Task<IActionResult> GetAllServices([FromQuery] BaseQueryObject queryObject)
        {
            var result = await _serviceService.GetAllServices(queryObject);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(
                result.Status,
                new SuccessResponseDto
                {
                    Data = result.Data!.Select(service => service.ToServiceDto()),
                    Total = result.Total,
                    Took = result.Took,
                }
            );
        }

        // Lấy thông tin service theo ID
        [HttpGet("{serviceId:int}")]
        public async Task<IActionResult> GetServiceById([FromRoute] int serviceId)
        {
            var result = await _serviceService.GetServiceById(serviceId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Data = result.Data!.ToServiceDto() });
        }

        // Chỉ Admin mới có quyền tạo service mới
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateNewService([FromBody] CreateUpdateServiceDto createServiceDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            // Lấy ID người dùng đã đăng nhập (Admin)
            var authUserId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (authUserId == null)
            {
                return StatusCode(ResStatusCode.UNAUTHORIZED, new ErrorResponseDto { Message = "Unauthorized" });
            }

            var result = await _serviceService.CreateNewService(createServiceDto, int.Parse(authUserId));
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        // Chỉ Admin mới có quyền cập nhật service
        [Authorize(Roles = "Admin")]
        [HttpPatch("{serviceId:int}")]
        public async Task<IActionResult> UpdateService([FromRoute] int serviceId, [FromBody] CreateUpdateServiceDto updateServiceDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var result = await _serviceService.UpdateService(serviceId, updateServiceDto);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        // Chỉ Admin mới có quyền xóa service
        [Authorize(Roles = "Admin")]
        [HttpDelete("{serviceId:int}")]
        public async Task<IActionResult> DeleteService([FromRoute] int serviceId)
        {
            var result = await _serviceService.DeleteService(serviceId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }
    }
}
