using System.Linq;
using server.Dtos.Feature; // Import DTOs cho Feature
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
                CreatedBy = feature.CreatedBy == null ? null : new AdminInfo
                {
                    Id = feature.CreatedBy.Id,
                    FirstName = feature.CreatedBy.FirstName,
                    LastName = feature.CreatedBy.LastName,
                    Email = feature.CreatedBy.Email,
                },
                RoomClasses = feature.RoomClassFeatures == null
                    ? new List<FeatureRoomClassInfo>()
                    : feature.RoomClassFeatures.Select(rcf => new FeatureRoomClassInfo
                    {
                        RoomClassId = rcf.RoomClassId,
                        Name = rcf.RoomClass?.ClassName, // Lấy ClassName từ RoomClass liên kết với RoomClassFeature
                        Quantity = rcf.Quantity,
                    }).ToList()
            };
        }
    }
}
