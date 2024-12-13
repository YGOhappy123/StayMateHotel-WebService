using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Room;
using server.Dtos.RoomClass;
using server.Models;

namespace server.Extensions.Mappers
{
    public static class RoomClassMapper
    {
        // Phương thức để chuyển đổi từ RoomClass sang RoomClassDto
        public static RoomClassDto ToRoomClassDto(this RoomClass roomclassModel)
        {
            return new RoomClassDto
            {
                Id = roomclassModel.Id,
                ClassName = roomclassModel.ClassName,
                BasePrice = roomclassModel.BasePrice,
                Capacity = roomclassModel.Capacity,
                CreatedAt = roomclassModel.CreatedAt,
                CreateById = roomclassModel.CreatedById,
                CreatedBy =
                    roomclassModel?.CreatedBy == null
                        ? null
                        : new AdminInfo
                        {
                            Id = roomclassModel.CreatedBy.Id,
                            FirstName = roomclassModel.CreatedBy.FirstName,
                            LastName = roomclassModel.CreatedBy.LastName,
                            Email = roomclassModel.CreatedBy.Email,
                        },

                Features =
                    roomclassModel?.RoomClassFeatures == null
                        ? null
                        : roomclassModel
                            .RoomClassFeatures.Select(ft => new RoomClassFeatureInfo
                            {
                                FeatureId = ft.FeatureId,
                                Name = ft.Feature?.Name,
                                Quantity = ft.Quantity,
                            })
                            .ToList(),
            };
        }
    }
}
