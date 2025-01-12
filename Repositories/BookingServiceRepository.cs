using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Enums;
using server.Interfaces.Repositories;
using server.Models;
using server.Queries;
using server.Utilities;

namespace server.Repositories
{
    public class BookingServiceRepository : IBookingServiceRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public BookingServiceRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        private IQueryable<BookingService> ApplyFilters(IQueryable<BookingService> query, Dictionary<string, object> filters)
        {
            foreach (var filter in filters)
            {
                string value = filter.Value.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(value))
                {
                    switch (filter.Key)
                    {
                        case "customerName":
                            var nameParts = value.Split(' '); 
                            query = query.Where(bks => nameParts.All(part =>
                                bks.Booking.Guest.FirstName.Contains(part) ||
                                bks.Booking.Guest.LastName.Contains(part)
                            ));
                            break;
                        case "bookingServiceId":
                            query = query.Where(bks => bks.Id == Convert.ToInt32(value)); 
                            break;
                        case "bookingId":
                            query = query.Where(bks => bks.BookingId == Convert.ToInt32(value));
                            break;
                        case "serviceName":
                            query = query.Where(bks => bks.Service.Name.Contains(value));
                            break;
                        case "startTime":
                            query = query.Where(bks => bks.CreatedAt >= DateTime.Parse(value));
                            break;
                        case "endTime":
                            query = query.Where(bks =>
                                bks.CreatedAt <= TimestampHandler.GetEndOfTimeByType(DateTime.Parse(value), "daily")
                            );
                            break;
                        case "minPrice":
                            query = query.Where(bks => bks.UnitPrice * bks.Quantity >= Convert.ToDecimal(value));
                            break;
                        case "maxPrice":
                            query = query.Where(bks => bks.UnitPrice * bks.Quantity <= Convert.ToDecimal(value));
                            break;
                        case "status":
                            query = query.Where(bks => bks.Status == Enum.Parse<BookingServiceStatus>(value));
                            break;
                        default:
                            query = query.Where(bks => EF.Property<string>(bks, filter.Key.CapitalizeWord()) == value);
                            break;
                    }
                }
            }

            return query;
        }

        private IQueryable<BookingService> ApplySorting(IQueryable<BookingService> query, Dictionary<string, string> sort)
        {
            foreach (var order in sort)
            {
                switch (order.Key.ToLower())
                {
                    case "price":
                        query = order.Value == "ASC"
                            ? query.OrderBy(mt => mt.UnitPrice * mt.Quantity) 
                            : query.OrderByDescending(mt => mt.UnitPrice * mt.Quantity); 
                        break;

                    // Các trường hợp sắp xếp khác
                    default:
                        query = order.Value == "ASC"
                            ? query.OrderBy(mt => EF.Property<object>(mt, order.Key.CapitalizeWord()))
                            : query.OrderByDescending(mt => EF.Property<object>(mt, order.Key.CapitalizeWord()));
                        break;
                }
            }

            return query;
        }

        public async Task<(List<BookingService>, int)> GetAllBookingServices(BaseQueryObject queryObject)
        {
            var query = _dbContext
                .BookingServices.Include(bks => bks.Booking)
                .ThenInclude(bk => bk.Guest)
                .Include(bks => bks.Service)
                .AsQueryable();

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

            var bookingServices = await query.ToListAsync();

            return (bookingServices, total);
        }

        public async Task<BookingService?> GetBookingServiceById(int bookingServiceId)
        {
            return await _dbContext
                .BookingServices.Include(bks => bks.Booking)
                .ThenInclude(bk => bk.Guest)
                .Include(bks => bks.Service)
                .Where(bks => bks.Id == bookingServiceId)
                .FirstOrDefaultAsync();
        }

        public async Task CreateNewBookingService(BookingService bookingService)
        {
            _dbContext.BookingServices.Add(bookingService);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBookingService(BookingService bookingService)
        {
            _dbContext.BookingServices.Update(bookingService);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CountBookingsByStatus(BookingServiceStatus status, TimeRangeQueryObject queryObject)
        {
            var query = _dbContext.BookingServices.Where(bk => bk.Status == status).AsQueryable();

            if (queryObject.StartTime != null)
            {
                query = query.Where(bk => bk.CreatedAt >= queryObject.StartTime.Value);
            }
            if (queryObject.EndTime != null)
            {
                query = query.Where(bk => bk.CreatedAt <= TimestampHandler.GetEndOfTimeByType(queryObject.EndTime.Value, "daily"));
            }

            return await query.CountAsync();
        }
    }
}
