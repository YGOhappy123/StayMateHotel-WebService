using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Response;
using server.Dtos.Room;
using server.Extensions.Mappers;
using server.Interfaces.Services;
using server.Queries;
using server.Utilities;

namespace server.Controllers
{
    [ApiController]
    [Route("/rooms")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms([FromQuery] BaseQueryObject queryObject)
        {
            var result = await _roomService.GetAllRooms(queryObject);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(
                result.Status,
                new SuccessResponseDto
                {
                    Data = result.Data!.Select(rm => rm.ToRoomDto()),
                    Total = result.Total,
                    Took = result.Took,
                }
            );
        }

        [HttpGet("{roomId:int}")]
        public async Task<IActionResult> GetRoomById([FromRoute] int roomId)
        {
            var result = await _roomService.GetRoomById(roomId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Data = result.Data });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateNewRoom([FromBody] CreateUpdateRoomDto createRoomDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var authUserId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var result = await _roomService.CreateNewRoom(createRoomDto, int.Parse(authUserId!));
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{roomId:int}")]
        public async Task<IActionResult> UpdateRoom([FromRoute] int roomId, [FromBody] CreateUpdateRoomDto updateRoomDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var result = await _roomService.UpdateRoom(roomId, updateRoomDto);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{roomId:int}")]
        public async Task<IActionResult> DeleteRoom([FromRoute] int roomId)
        {
            var result = await _roomService.DeleteRoom(roomId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("toggle-maintenance/{roomId:int}")]
        public async Task<IActionResult> ToggleMaintenance([FromRoute] int roomId)
        {
            var result = await _roomService.ToggleMaintenance(roomId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("cleaning-done/{roomId:int}")]
        public async Task<IActionResult> MarkCleaningDone([FromRoute] int roomId)
        {
            var result = await _roomService.MarkCleaningDone(roomId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }
    }
}
