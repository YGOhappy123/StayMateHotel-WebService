using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Floor;
using server.Models;

namespace server.Extensions.Mappers
{
    public static class FloorMapper
    {
        public static FloorDto ToFloorDto(this Floor floor)
        {
            return new FloorDto
            {
                Id = floor.Id,
                FloorNumber = floor.FloorNumber,
                Rooms = floor.Rooms.Select(r => r.RoomNumber).ToList(),
            };
        }
    }
}
