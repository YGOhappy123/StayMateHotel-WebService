using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Response;
using server.Dtos.Feature;
using server.Interfaces.Services;
using server.Models;
using server.Utilities;
using server.Queries;
using server.Extensions.Mappers;

namespace server.Controllers
{
    [ApiController]
    [Route("/features")]
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureService _featureService;

        public FeatureController(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        // Lấy tất cả các Feature với phân trang hoặc lọc (BaseQueryObject)
        [HttpGet]
        public async Task<IActionResult> GetAllFeatures([FromQuery] BaseQueryObject queryObject)
        {
            var result = await _featureService.GetAllFeatures(queryObject);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(
                result.Status,
                new SuccessResponseDto
                {
                    Data = result.Data!.Select(feature => feature.ToFeatureDto()),
                    Total = result.Total,
                    Took = result.Took,
                }
            );
        }

        // Lấy thông tin tính năng theo ID
        [HttpGet("{featureId:int}")]
        public async Task<IActionResult> GetFeatureById([FromRoute] int featureId)
        {
            var result = await _featureService.GetFeatureById(featureId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Data = result.Data!.ToFeatureDto() });
        }

        // Chỉ Admin mới có quyền tạo tính năng mới
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateNewFeature([FromBody] CreateUpdateFeatureDto createFeatureDto)
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

            var result = await _featureService.CreateNewFeature(createFeatureDto, int.Parse(authUserId));
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        // Chỉ Admin mới có quyền cập nhật tính năng
        [Authorize(Roles = "Admin")]
        [HttpPatch("{featureId:int}")]
        public async Task<IActionResult> UpdateFeature([FromRoute] int featureId, [FromBody] CreateUpdateFeatureDto updateFeatureDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var result = await _featureService.UpdateFeature(featureId, updateFeatureDto);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        // Chỉ Admin mới có quyền xóa tính năng
        [Authorize(Roles = "Admin")]
        [HttpDelete("{featureId:int}")]
        public async Task<IActionResult> DeleteFeature([FromRoute] int featureId)
        {
            var result = await _featureService.DeleteFeature(featureId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }
    }
}
