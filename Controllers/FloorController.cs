using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using server.Dtos.Response;
using server.Extensions.Mappers;
using server.Interfaces.Repositories;
using server.Utilities;
using server.Dtos.Floor;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("/floors")]
    public class FloorController : ControllerBase
    {
        private readonly IFloorRepository _floorRepo;
        private readonly ILogger<FloorController> _logger;

        public FloorController(IFloorRepository floorRepo, ILogger<FloorController> logger)
        {
            _floorRepo = floorRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFloors()
        {
            var floors = await _floorRepo.GetAllFloors();

            return StatusCode(ResStatusCode.OK, new SuccessResponseDto { Data = floors.Select(floor => floor.ToFloorDto()) });
        }

        [HttpGet("{floorId:int}", Name = "GetFloorById")]
        public async Task<IActionResult> GetFloorByIdAsync([FromRoute] int floorId)
        {
            var floor = await _floorRepo.GetFloorById(floorId);

            if (floor == null)
            {
                return StatusCode(ResStatusCode.NOT_FOUND, new ErrorResponseDto { Message = ErrorMessage.FLOOR_NOT_FOUND });
            }

            return StatusCode(ResStatusCode.OK, new SuccessResponseDto { Data = floor.ToFloorDto() });
        }

        [HttpPost]
        public async Task<IActionResult> AddFloorAsync([FromBody] FloorDto floorDto)
        {
            // Tạo mới đối tượng Floor từ FloorDto
            var floor = new Floor 
            {
                FloorNumber = floorDto.FloorNumber
                // Ánh xạ các trường khác nếu cần
            };
            
            var addedFloor = await _floorRepo.AddFloor(floor);
            
            if (addedFloor?.Id == null)
            {
                return BadRequest(new { Message = "Failed to create floor: ID is null" });
            }

            return CreatedAtRoute(
                routeName: "GetFloorById", 
                new { floorId = addedFloor.Id }, 
                addedFloor.ToFloorDto() // Chuyển đổi lại sang DTO để trả về
            );
        }

        [HttpPut("{floorId:int}")]
        public async Task<IActionResult> UpdateFloorAsync(
            [FromRoute] int floorId, 
            [FromBody] FloorDto floorDto)
        {
            // Cập nhật
            var existingFloor = await _floorRepo.GetFloorById(floorId);
            if (existingFloor == null)
                return NotFound();

            existingFloor.FloorNumber = floorDto.FloorNumber;
            var result = await _floorRepo.UpdateFloor(existingFloor);
            
            return Ok(new 
            { 
                message = "Floor updated successfully", 
                floor = existingFloor 
            });
        }

        [HttpDelete("{floorId:int}")]
        public async Task<IActionResult> DeleteFloorAsync([FromRoute] int floorId)
        {
            try 
            {
                var existingFloor = await _floorRepo.GetFloorById(floorId);
                if (existingFloor == null)
                {
                    return NotFound(new ErrorResponseDto 
                    { 
                        Message = ErrorMessage.FLOOR_NOT_FOUND 
                    });
                }

                var result = await _floorRepo.DeleteFloor(floorId);
                if (!result)
                {
                    return StatusCode(500, new ErrorResponseDto 
                    { 
                        Message = "Failed to delete floor" 
                    });
                }

                return Ok(new SuccessResponseDto 
                { 
                    Message = "Floor deleted successfully" 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting floor");

                // Ghi log lỗi ở đây nếu cần
                return StatusCode(500, new ErrorResponseDto 
                { 
                    Message = "An error occurred while deleting the floor"
                });
            }
        }
    }
}