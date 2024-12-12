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
    public class FloorRepository : IFloorRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public FloorRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        private IQueryable<Floor> ApplyFilters(IQueryable<Floor> query, Dictionary<string, object> filters)
        {
            foreach (var filter in filters)
            {
                string value = filter.Value.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(value))
                {
                    switch (filter.Key)
                    {
                        case "startTime":
                            query = query.Where(f => f.CreatedAt >= DateTime.Parse(value));
                            break;
                        case "endTime":
                            query = query.Where(f => f.CreatedAt <= TimestampHandler.GetEndOfTimeByType(DateTime.Parse(value), "daily"));
                            break;
                        case "floorNumber":
                            query = query.Where(f => f.FloorNumber.Contains(value));
                            break;
                        default:
                            query = query.Where(f => EF.Property<string>(f, filter.Key.CapitalizeWord()) == value);
                            break;
                    }
                }
            }

            return query;
        }

        private IQueryable<Floor> ApplySorting(IQueryable<Floor> query, Dictionary<string, string> sort)
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

        public async Task<(List<Floor>, int)> GetAllFloors(BaseQueryObject queryObject)
        {
            var query = _dbContext.Floors.Include(f => f.Rooms).Include(f => f.CreatedBy).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject.Filter))
            {
                var parsedFilter = JsonSerializer.Deserialize<Dictionary<string, object>>(queryObject.Filter);
                query = ApplyFilters(query, parsedFilter!);
            }

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

            var floors = await query.ToListAsync();

            return (floors, total);
        }

        public async Task<Floor?> GetFloorById(int floorId)
        {
            return await _dbContext
                .Floors.Include(f => f.Rooms)
                .Include(f => f.CreatedBy)
                .Where(f => f.Id == floorId)
                .FirstOrDefaultAsync();
        }

        public async Task<Floor?> GetFloorsByFloorNumber(string floorNumber)
        {
            return await _dbContext.Floors.Where(f => f.FloorNumber == floorNumber).FirstOrDefaultAsync();
        }

        public async Task CreateNewFloor(Floor floor)
        {
            _dbContext.Floors.Add(floor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateFloor(Floor floor)
        {
            _dbContext.Floors.Update(floor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteFloor(Floor floor)
        {
            _dbContext.Floors.Remove(floor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CountRoomsInFloor(int floorId)
        {
            return await _dbContext.Rooms.Where(rif => rif.FloorId == floorId).CountAsync();
        }
    }
}
