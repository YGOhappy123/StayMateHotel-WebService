using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Room;
using server.Models;

namespace server.Extensions.Mappers
{
    public static class RoomMapper
    {
        public static RoomDto ToRoomDto(this Room roomModel)
        {
            return new RoomDto
            {
                Id = roomModel.Id,
                RoomNumber = roomModel.RoomNumber,
                Status = roomModel.Status,
                FloorId = roomModel.FloorId,
                RoomClassId = roomModel.RoomClassId,
            };
        }
    }
}
