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
    [ApiController]
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

            return StatusCode(
                ResStatusCode.OK,
                new SuccessResponseDto { Data = rooms.Select(room => room.ToRoomDto()) }
            );
        }

        [HttpGet("{roomId:int}")]
        public async Task<IActionResult> GetRoomByIdAsync([FromRoute] int roomId)
        {
            var room = await _roomRepo.GetRoomById(roomId);

            if (room == null)
            {
                return StatusCode(
                    ResStatusCode.NOT_FOUND,
                    new ErrorResponseDto { Message = ErrorMessage.ROOM_NOT_FOUND }
                );
            }

            return StatusCode(ResStatusCode.OK, new SuccessResponseDto { Data = room.ToRoomDto() });
        }
    }
}
