using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using server.Dtos.Response;
using server.Dtos.Service; // Import Dto cho Service
using server.Models;
using server.Queries;

namespace server.Interfaces.Services
{
    public interface IServiceService
    {
        // Lấy tất cả các Service với phân trang hoặc lọc (BaseQueryObject)
        Task<ServiceResponse<List<Service>>> GetAllServices(BaseQueryObject queryObject);

        // Lấy thông tin dịch vụ theo ID
        Task<ServiceResponse<Service>> GetServiceById(int serviceId);

        // Tạo dịch vụ mới
        Task<ServiceResponse> CreateNewService(CreateUpdateServiceDto createServiceDto, int adminId);

        // Cập nhật dịch vụ
        Task<ServiceResponse> UpdateService(int serviceId, CreateUpdateServiceDto updateServiceDto);

        // Xóa dịch vụ
        Task<ServiceResponse> DeleteService(int serviceId);
    }
}
