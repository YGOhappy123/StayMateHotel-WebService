using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Interfaces.Repositories;
using server.Mappers;

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

            return Ok(rooms.Select(room => room.ToRoomDto()));
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetRoomByIdAsync([FromRoute] int roomId)
        {
            var room = await _roomRepo.GetRoomById(roomId);

            if (room == null)
            {
                return NotFound("ROOM_NOT_FOUND");
            }
            else
            {
                return Ok(room.ToRoomDto());
            }
        }
    }
}
