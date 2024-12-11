using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Floor;
using server.Dtos.Response;
using server.Extensions.Mappers;
using server.Interfaces.Services;
using server.Queries;
using server.Utilities;

namespace server.Controllers
{
    [ApiController]
    [Route("/floors")]
    public class FloorController : ControllerBase
    {
        private readonly IFloorService _floorService;

        public FloorController(IFloorService floorService)
        {
            _floorService = floorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFloors([FromQuery] BaseQueryObject queryObject)
        {
            var result = await _floorService.GetAllFloors(queryObject);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(
                result.Status,
                new SuccessResponseDto
                {
                    Data = result.Data!.Select(f => f.ToFloorDto()),
                    Total = result.Total,
                    Took = result.Took,
                }
            );
        }

        [HttpGet("{floorId:int}")]
        public async Task<IActionResult> GetFloorById([FromRoute] int floorId)
        {
            var result = await _floorService.GetFloorById(floorId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Data = result.Data!.ToFloorDto() });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateNewFloor([FromBody] CreateUpdateFloorDto createFloorDto)
        {
            // Kiểm tra dữ liệu đầu vào
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var authUserId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var result = await _floorService.CreateNewFloor(createFloorDto, int.Parse(authUserId!));
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{floorId:int}")]
        public async Task<IActionResult> UpdateFloor([FromRoute] int floorId, [FromBody] CreateUpdateFloorDto updateFloorDto)
        {
            // Kiểm tra dữ liệu đầu vào
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var result = await _floorService.UpdateFloor(floorId, updateFloorDto);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{floorId:int}")]
        public async Task<IActionResult> DeleteFloor([FromRoute] int floorId)
        {
            // Kiểm tra dữ liệu đầu vào
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var result = await _floorService.DeleteFloor(floorId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }
    }
}
