using System.Linq;
using server.Dtos.Service;  // Nhập các DTO cho Service
using server.Models;  // Nhập các Models

namespace server.Extensions.Mappers
{
    public static class ServiceMapper
    {
        // Ánh xạ từ model Service sang ServiceDto
        public static ServiceDto ToServiceDto(this Service service)
        {
            return new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Price = service.Price,
                IsAvailable = service.IsAvailable,
                CreatedAt = service.CreatedAt,
                CreatedById = service.CreatedById,
                // Kiểm tra null khi ánh xạ CreatedBy
                CreatedBy = service.CreatedBy == null ? null : new UserInfo
                {
                    Id = service.CreatedBy.Id,
                    FirstName = service.CreatedBy.FirstName,
                    LastName = service.CreatedBy.LastName,
                    Email = service.CreatedBy.Email,
                }
            };
        }
    }
}
