using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces.Repositories;
using server.Models;
using server.Queries;
using server.Utilities;

namespace server.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public ServiceRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        // Áp dụng bộ lọc cho danh sách dịch vụ
        private IQueryable<Service> ApplyFilters(IQueryable<Service> query, Dictionary<string, object> filters)
        {
            foreach (var filter in filters)
            {
                string value = filter.Value.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(value))
                {
                    switch (filter.Key)
                    {
                        case "name":
                            query = query.Where(s => s.Name.Contains(value));
                            break;
                        case "isAvailable":
                            query = query.Where(s => s.IsAvailable == Convert.ToBoolean(value));
                            break;
                        case "createdById":
                            query = query.Where(s => s.CreatedById == Convert.ToInt32(value));
                            break;
                        case "startTime":
                            query = query.Where(s => s.CreatedAt >= DateTime.Parse(value));
                            break;
                        case "endTime":
                            query = query.Where(s => s.CreatedAt <= DateTime.Parse(value));
                            break;
                        default:
                            query = query.Where(s => EF.Property<string>(s, filter.Key.CapitalizeWord()) == value);
                            break;
                    }
                }
            }

            return query;
        }

        // Áp dụng sắp xếp cho danh sách dịch vụ
        private IQueryable<Service> ApplySorting(IQueryable<Service> query, Dictionary<string, string> sort)
        {
            foreach (var order in sort)
            {
                if (order.Key == "name")
                {
                    query = order.Value == "ASC"
                        ? query.OrderBy(s => s.Name)
                        : query.OrderByDescending(s => s.Name);
                }
                else
                {
                    query = order.Value == "ASC"
                        ? query.OrderBy(s => EF.Property<object>(s, order.Key.CapitalizeWord()))
                        : query.OrderByDescending(s => EF.Property<object>(s, order.Key.CapitalizeWord()));
                }
            }

            return query;
        }

        // Lấy tất cả các dịch vụ với các bộ lọc và sắp xếp
        public async Task<(List<Service>, int)> GetAllServices(BaseQueryObject queryObject)
        {
            var query = _dbContext
                .Services.Include(s => s.CreatedBy) // Ánh xạ với CreatedBy nếu cần
                .AsQueryable();

            // Áp dụng bộ lọc nếu có
            if (!string.IsNullOrWhiteSpace(queryObject.Filter))
            {
                var parsedFilter = JsonSerializer.Deserialize<Dictionary<string, object>>(queryObject.Filter);
                query = ApplyFilters(query, parsedFilter!);
            }

            // Áp dụng sắp xếp nếu có
            if (!string.IsNullOrWhiteSpace(queryObject.Sort))
            {
                var parsedSort = JsonSerializer.Deserialize<Dictionary<string, string>>(queryObject.Sort);
                query = ApplySorting(query, parsedSort!);
            }

            var total = await query.CountAsync();

            // Phân trang
            if (queryObject.Skip.HasValue)
                query = query.Skip(queryObject.Skip.Value);

            if (queryObject.Limit.HasValue)
                query = query.Take(queryObject.Limit.Value);

            var services = await query.ToListAsync();

            return (services, total);
        }

        // Lấy dịch vụ theo ID
        public async Task<Service?> GetServiceById(int serviceId)
        {
            return await _dbContext.Services
                .Include(s => s.CreatedBy)  // Ánh xạ với CreatedBy nếu cần
                .Where(s => s.Id == serviceId)
                .FirstOrDefaultAsync();
        }

        // Lấy dịch vụ theo tên
        public async Task<Service?> GetServiceByName(string serviceName)
        {
            return await _dbContext.Services
                .Where(s => s.Name == serviceName)
                .FirstOrDefaultAsync();
        }

        // Tạo dịch vụ mới
        public async Task CreateNewService(Service service)
        {
            _dbContext.Services.Add(service);
            await _dbContext.SaveChangesAsync();
        }

        // Cập nhật thông tin dịch vụ
        public async Task UpdateService(Service service)
        {
            _dbContext.Services.Update(service);
            await _dbContext.SaveChangesAsync();
        }

        // Xóa dịch vụ
        public async Task DeleteService(Service service)
        {
            _dbContext.Services.Remove(service);
            await _dbContext.SaveChangesAsync();
        }
    }
}
