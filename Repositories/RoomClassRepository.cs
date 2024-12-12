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
    public class RoomClassRepository : IRoomClassRepository
    {
        private readonly ApplicationDBContext _dbContext;

        // Constructor để nhận vào AppDbContext
        public RoomClassRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        private IQueryable<RoomClass> ApplyFilters(IQueryable<RoomClass> query, Dictionary<string, object> filters)
        {
            foreach (var filter in filters)
            {
                string value = filter.Value.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(value))
                {
                    switch (filter.Key)
                    {
                        case "startTime":
                            query = query.Where(rmc => rmc.CreatedAt >= DateTime.Parse(value));
                            break;
                        case "endTime":
                            query = query.Where(rmc => rmc.CreatedAt <= TimestampHandler.GetEndOfTimeByType(DateTime.Parse(value), "daily"));
                            break;
                        case "roomClass":
                            query = query.Where(rmc => rmc.ClassName.Contains(value));
                            break;
                        case "minPrice":
                            query = query.Where(rmc => rmc.BasePrice >= Convert.ToDecimal(value));
                            break;
                        case "maxPrice":
                            query = query.Where(rmc => rmc.BasePrice <= Convert.ToDecimal(value));
                            break;
                        case "minCapacity":
                            query = query.Where(rmc => rmc.Capacity >= Convert.ToDecimal(value));
                            break;
                        case "maxCapacity":
                            query = query.Where(rmc => rmc.Capacity <= Convert.ToDecimal(value));
                            break;
                        case "features":
                            var featureIds = JsonSerializer.Deserialize<List<int>>(filter.Value.ToString() ?? "[]");
                            query = query.Where(rmc =>
                                featureIds!.All(featureId => rmc.RoomClassFeatures.Any(rcf => rcf.FeatureId == featureId))
                            );
                            break;
                        default:
                            query = query.Where(rmc => EF.Property<string>(rmc, filter.Key.CapitalizeWord()) == value);
                            break;
                    }
                }
            }

            return query;
        }

        private IQueryable<RoomClass> ApplySorting(IQueryable<RoomClass> query, Dictionary<string, string> sort)
        {
            foreach (var order in sort)
            {
                
                query =
                    order.Value == "ASC"
                        ? query.OrderBy(mt => EF.Property<object>(mt, order.Key.CapitalizeWord()))
                        : query.OrderByDescending(mt => EF.Property<object>(mt, order.Key.CapitalizeWord()));
                
            }

            return query;
        }

        // Lấy tất cả RoomClass
        public async Task<(List<RoomClass>, int)> GetAllRoomClasses(BaseQueryObject queryObject)
        {
            var query = _dbContext
                .RoomClasses
                .Include(rmc => rmc.CreatedBy)
                .Include(rmc => rmc.Rooms)
                .Include(rmc => rmc.RoomClassFeatures)
                .ThenInclude(rcf => rcf.Feature)
                .AsQueryable();

            // Filter
            if (!string.IsNullOrEmpty(queryObject.Filter))
            {
                var parsedFilter = JsonSerializer.Deserialize<Dictionary<string, object>>(queryObject.Filter);
                query = ApplyFilters(query, parsedFilter!);            }

            // Sort
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

            var roomClasses = await query.ToListAsync();

            return (roomClasses, total);
        }


        public async Task<RoomClass?> GetRoomClassById(int roomClassId)
        {
            return await _dbContext
                .RoomClasses
                .Include(rmc => rmc.Rooms)
                .Include(rmc => rmc.RoomClassFeatures)
                .ThenInclude(rcf => rcf.Feature)
                .Where(rmc => rmc.Id == roomClassId)
                .FirstOrDefaultAsync();
        }

        public async Task<RoomClass?> GetRoomClassByName(string roomClassName)
        {
            return await _dbContext.RoomClasses.Where(rmc => rmc.ClassName == roomClassName).FirstOrDefaultAsync();
        }

        public async Task CreateNewRoomClass(RoomClass roomClass)
        {
            _dbContext.RoomClasses.Add(roomClass);
            await _dbContext.SaveChangesAsync();
        }

        // Cập nhật RoomClass
        public async Task UpdateRoomClass(RoomClass roomClass)
        {
            _dbContext.RoomClasses.Update(roomClass);  // Cập nhật phòng học trong DbContext
            await _dbContext.SaveChangesAsync();  // Lưu thay đổi vào cơ sở dữ liệu
        }

        // Xóa RoomClass theo Id
        public async Task DeleteRoomClass(RoomClass roomClass)
        {
            _dbContext.RoomClasses.Remove(roomClass);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CountRoomsInRoomClass(int roomClassId)
        {
            return await _dbContext.Rooms.Where(rm => rm.RoomClassId == roomClassId).CountAsync();
        }

        public async Task DeleteFeatureOfRoomClass(int roomClassId)
        {
            var features = await _dbContext.RoomClassFeatures.Where(rcf => rcf.RoomClassId == roomClassId).ToListAsync();

            _dbContext.RoomClassFeatures.RemoveRange(features);
            await _dbContext.SaveChangesAsync();
        }
    }
}
