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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public PaymentRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        private IQueryable<Payment> ApplyFilters(IQueryable<Payment> query, Dictionary<string, object> filters)
        {
            foreach (var filter in filters)
            {
                string value = filter.Value.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(value))
                {
                    switch (filter.Key)
                    {
                        case "startTime":
                            query = query.Where(pm => pm.PaymentTime >= DateTime.Parse(value));
                            break;
                        case "endTime":
                            query = query.Where(pm =>
                                pm.PaymentTime <= TimestampHandler.GetEndOfTimeByType(DateTime.Parse(value), "daily")
                            );
                            break;
                        case "method":
                            query = query.Where(pm => pm.Method == Enum.Parse<PaymentMethod>(value));
                            break;
                        default:
                            query = query.Where(pm => EF.Property<string>(pm, filter.Key.CapitalizeWord()) == value);
                            break;
                    }
                }
            }

            return query;
        }

        private IQueryable<Payment> ApplySorting(IQueryable<Payment> query, Dictionary<string, string> sort)
        {
            foreach (var order in sort)
            {
                if (order.Key == "CreatedAt")
                {
                    query = order.Value == "ASC" ? query.OrderBy(mt => mt.PaymentTime) : query.OrderByDescending(mt => mt.PaymentTime);
                }
                else
                {
                    query =
                        order.Value == "ASC"
                            ? query.OrderBy(mt => EF.Property<object>(mt, order.Key.CapitalizeWord()))
                            : query.OrderByDescending(mt => EF.Property<object>(mt, order.Key.CapitalizeWord()));
                }
            }

            return query;
        }

        public async Task<(List<Payment>, int)> GetAllPayments(BaseQueryObject queryObject)
        {
            var query = _dbContext.Payments.Include(pm => pm.Booking).AsQueryable();

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

            var payments = await query.ToListAsync();

            return (payments, total);
        }

        public Task<decimal> GetBookingToTalPayments(int bookingId)
        {
            return _dbContext.Payments.Where(pm => pm.BookingId == bookingId).SumAsync(pm => pm.Amount);
        }

        public async Task MakeNewPayment(Payment payment)
        {
            _dbContext.Payments.Add(payment);
            await _dbContext.SaveChangesAsync();
        }
    }
}
