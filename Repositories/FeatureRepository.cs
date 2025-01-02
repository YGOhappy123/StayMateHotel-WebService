using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HandlebarsDotNet;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces.Repositories;
using server.Models;
using server.Queries;
using server.Utilities;

namespace server.Repositories
{
    public class FeatureRepository : IFeatureRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public FeatureRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }
       
        private IQueryable<Feature> ApplyFilters(IQueryable<Feature> query, Dictionary<string, object> filters)
        {
            foreach (var filter in filters)
            {
                string value = filter.Value.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(value))
                {
                    switch (filter.Key)
                    {
                        case "name":
                            query = query.Where(f => f.Name.Contains(value));
                            break;
                        case "createdById":
                            query = query.Where(f => f.CreatedById == Convert.ToInt32(value));
                            break;
                        case "startTime":
                            query = query.Where(f => f.CreatedAt >= DateTime.Parse(value));
                            break;
                        case "endTime":
                            query = query.Where(f => f.CreatedAt <= DateTime.Parse(value));
                            break;
                        case "roomClasses":
                            var roomClassIds = JsonSerializer.Deserialize<List<int>>(filter.Value.ToString() ?? "[]");

                            //query = query.Where(f =>
                            //    f.RoomClassFeatures.All(rmc =>
                            //        roomClassIds!.Contains(rmc.RoomClassId.GetValueOrDefault()) // Lấy giá trị của RoomClassId nếu có
                            //    )
//=======
                            query = query.Where(feature =>
                                roomClassIds!.All(roomClassId => feature.RoomClassFeatures.Any(rcf => rcf.RoomClassId == roomClassId))
//>>>>>>> main
                            );
                            break;
                        default:
                            query = query.Where(f => EF.Property<string>(f, filter.Key.CapitalizeWord()) == value);
                            break;
                    }
                }
            }

            return query;
        }

        // Áp dụng sắp xếp cho danh sách Feature
        private IQueryable<Feature> ApplySorting(IQueryable<Feature> query, Dictionary<string, string> sort)
        {
            foreach (var order in sort)
            {
                if (order.Key == "name")
                {
                    query = order.Value == "ASC"
                        ? query.OrderBy(f => f.Name)
                        : query.OrderByDescending(f => f.Name);
                }
                else if (order.Key == "roomClassFeatures") // Sắp xếp theo số lượng phần tử
                {
                    query = order.Value == "ASC"
                        ? query.OrderBy(f => f.RoomClassFeatures.Count)
                        : query.OrderByDescending(f => f.RoomClassFeatures.Count);
                }
                else
                {
                    query = order.Value == "ASC"
                        ? query.OrderBy(f => EF.Property<object>(f, order.Key.CapitalizeWord()))
                        : query.OrderByDescending(f => EF.Property<object>(f, order.Key.CapitalizeWord()));
                }
            }

            return query;
        }

        // Lấy tất cả Feature với các bộ lọc và sắp xếp
        public async Task<(List<Feature>, int)> GetAllFeatures(BaseQueryObject queryObject)
        {
            var query = _dbContext
                .Features.Include(f => f.CreatedBy) // Ánh xạ với CreatedBy nếu cần
                .Include(f => f.RoomClassFeatures)

                .ThenInclude(rcf => rcf.RoomClass)// Ánh xạ RoomClassFeatures nếu cần


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

            if (queryObject.Skip.HasValue)
                query = query.Skip(queryObject.Skip.Value);

            if (queryObject.Limit.HasValue)
                query = query.Take(queryObject.Limit.Value);

            var features = await query.ToListAsync();

            return (features, total);
        }

        // Lấy một Feature theo ID
        public async Task<Feature?> GetFeatureById(int featureId)
        {
            return await _dbContext.Features
                .Include(f => f.CreatedBy)
                .Include(f => f.RoomClassFeatures)
                .ThenInclude(rcf => rcf.RoomClass)
                .Where(f => f.Id == featureId)
                .FirstOrDefaultAsync();
        }

        // Lấy Feature theo tên
        public async Task<Feature?> GetFeatureByName(string featureName)
        {
            return await _dbContext.Features
                .Where(f => f.Name == featureName)
                .FirstOrDefaultAsync();
        }

        // Tạo mới một Feature
        public async Task CreateNewFeature(Feature feature)
        {
            _dbContext.Features.Add(feature);
            await _dbContext.SaveChangesAsync();
        }

        // Cập nhật một Feature
        public async Task UpdateFeature(Feature feature)
        {
            _dbContext.Features.Update(feature);
            await _dbContext.SaveChangesAsync();
        }

        // Xóa một Feature
        public async Task DeleteFeature(Feature feature)
        {
            _dbContext.Features.Remove(feature);
            await _dbContext.SaveChangesAsync();
        }
    }
}
