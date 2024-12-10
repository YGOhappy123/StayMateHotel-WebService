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
        public static RoomDto ToRoomDto(this Room room)
        {
            return new RoomDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Status = room.Status.ToString(),
                CreatedAt = room.CreatedAt,
                Images = room.Images.Select(img => img.ImageUrl).ToList(),
                FloorId = room?.FloorId,
                RoomClassId = room?.RoomClassId,
                CreatedBy =
                    room?.CreatedBy == null
                        ? null
                        : new UserInfo
                        {
                            Id = room.CreatedBy.Id,
                            FirstName = room.CreatedBy.FirstName,
                            LastName = room.CreatedBy.LastName,
                            Email = room.CreatedBy.Email,
                        },
                Floor = room?.Floor == null ? null : new RoomFloorInfo { Id = room.Floor.Id, FloorNumber = room.Floor.FloorNumber },
                RoomClass =
                    room?.RoomClass == null
                        ? null
                        : new RoomRoomClassInfo
                        {
                            Id = room.RoomClass.Id,
                            ClassName = room.RoomClass.ClassName,
                            BasePrice = room.RoomClass.BasePrice,
                            Capacity = room.RoomClass.Capacity,
                        },
                Features =
                    room?.RoomClass == null
                        ? null
                        : room
                            .RoomClass.RoomClassFeatures.Select(ft => new RoomFeatureInfo
                            {
                                Id = ft.FeatureId,
                                Name = ft.Feature?.Name,
                                Quantity = ft.Quantity,
                            })
                            .ToList(),
            };
        }
    }
}
