//using System.Linq;
//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Feature; // Import DTOs cho Feature
//using server.Dtos.Room;
using server.Models; // Import Models

namespace server.Extensions.Mappers
{
    public static class FeatureMapper
    {
        // Ánh xạ từ Feature sang FeatureDto
        public static FeatureDto ToFeatureDto(this Feature feature)
        {
            return new FeatureDto
            {
                Id = feature.Id,
                Name = feature.Name,
                CreatedAt = feature.CreatedAt,
                CreatedById = feature.CreatedById,
                CreatedBy = feature.CreatedBy == null ? null : new UserInfo
                {
                    Id = feature.CreatedBy.Id,
                    FirstName = feature.CreatedBy.FirstName,
                    LastName = feature.CreatedBy.LastName,
                    Email = feature.CreatedBy.Email,
                },

                RoomClass = feature?.RoomClassFeatures == null
                    ? null
                    : feature.RoomClassFeatures.Select(ft => new FeatureRoomClassInfo
                    {
                        Id = ft.RoomClassId,
                        ClassName = ft.RoomClass == null ? "ACSS" : ft.RoomClass?.ClassName,
                        Quantity = ft.Quantity,
                    }).ToList()
                
            };
        }
    }
}
