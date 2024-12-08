using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Response;
using server.Extensions.Mappers;
using server.Utilities;
using server.Dtos.Floor;
using server.Interfaces.Services;
using server.Queries;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("floors")]
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
                    Data = result.Data,
                }
            );
        }

        [HttpGet("{floorId:int}", Name = "GetFloorById")]
        public async Task<IActionResult> GetFloorById([FromRoute] int floorId)
        {
            var result = await _floorService.GetFloorById(floorId);

            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Data = result.Data!.ToFloorDto() });
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost("floors")]
        public async Task<IActionResult> AddNewFloor([FromBody] CreateFloorDto createFloorDto)
        {
            // Kiểm tra dữ liệu đầu vào
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }
            
            var result = await _floorService.CreateNewFloor(createFloorDto);
            
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        // [Authorize(Roles = "Admin")]
        [HttpPut("floors/{floorId:int}")]
        public async Task<IActionResult> UpdateFloorAsync([FromBody] UpdateFloorDto updateFloorDto, [FromRoute] int floorId)
        {
            // Kiểm tra dữ liệu đầu vào
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var result = await _floorService.UpdateFloor(updateFloorDto, floorId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });            
        }

        // [Authorize(Roles = "Admin")]
        [HttpDelete("floors/{floorId:int}")]
        public async Task<IActionResult> DeleteFloor([FromRoute] int floorId)
        {
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