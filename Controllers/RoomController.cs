using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Response;
using server.Interfaces.Repositories;
using server.Mappers;
using server.Utilities;

namespace server.Controllers
{
    [Route("/rooms")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository _roomRepo;

        public RoomController(IRoomRepository roomRepo)
        {
            _roomRepo = roomRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _roomRepo.GetAllRooms();

            return Ok(
                new SuccessResponseDto
                {
                    Status = 200,
                    Data = rooms.Select(room => room.ToRoomDto()),
                }
            );
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetRoomByIdAsync([FromRoute] int roomId)
        {
            var room = await _roomRepo.GetRoomById(roomId);

            if (room == null)
            {
                return NotFound(
                    new ErrorResponseDto { Status = 404, Message = ErrorMessage.ROOM_NOT_FOUND }
                );
            }

            return Ok(new SuccessResponseDto { Status = 200, Data = room.ToRoomDto() });
        }
    }
}
