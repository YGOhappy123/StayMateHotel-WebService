using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Interfaces;

namespace server.Controllers
{
    [Route("/rooms")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public IActionResult GetAllRooms()
        {
            var rooms = _roomService.GetAllRooms();
            return Ok(rooms);
        }

        [HttpGet("{roomId}")]
        public IActionResult GetRoomById([FromRoute] int roomId)
        {
            var room = _roomService.GetRoomById(roomId);

            if (room == null)
            {
                return NotFound("ROOM_NOT_FOUND");
            }
            else
            {
                return Ok(room);
            }
        }
    }
}
