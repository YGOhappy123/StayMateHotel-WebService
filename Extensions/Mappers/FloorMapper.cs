using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Floor;
using server.Dtos.Room;
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
                CreatedAt = floor.CreatedAt,
                Rooms = floor.Rooms.Select(r => r.RoomNumber).ToList(),
                CreatedBy =
                    floor?.CreatedBy == null
                        ? null
                        : new UserInfo
                        {
                            Id = floor.CreatedBy.Id,
                            FirstName = floor.CreatedBy.FirstName,
                            LastName = floor.CreatedBy.LastName,
                            Email = floor.CreatedBy.Email,
                        },
            };
        }
    }
}
