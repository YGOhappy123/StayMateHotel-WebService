using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using server.Models;
using server.Queries;

namespace server.Interfaces.Repositories
{
    public interface IServiceRepository
    {
        // Lấy tất cả các Service với phân trang hoặc lọc (BaseQueryObject)
        Task<(List<Service>, int)> GetAllServices(BaseQueryObject queryObject);

        // Lấy thông tin dịch vụ theo ID
        Task<Service?> GetServiceById(int serviceId);

        // Lấy dịch vụ theo tên (nếu có)
        Task<Service?> GetServiceByName(string serviceName);

        // Tạo dịch vụ mới
        Task CreateNewService(Service service);

        // Cập nhật dịch vụ
        Task UpdateService(Service service);

        // Xóa dịch vụ
        Task DeleteService(Service service);
    }
}
