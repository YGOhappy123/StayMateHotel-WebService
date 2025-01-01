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
    public class GuestRepository : IGuestRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public GuestRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        private IQueryable<Guest> ApplyFilters(IQueryable<Guest> query, Dictionary<string, object> filters)
        {
            foreach (var filter in filters)
            {
                string value = filter.Value.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(value))
                {
                    switch (filter.Key)
                    {
                        case "startTime":
                            query = query.Where(cus => cus.CreatedAt >= DateTime.Parse(value));
                            break;
                        case "endTime":
                            query = query.Where(cus => cus.CreatedAt <= DateTime.Parse(value));
                            break;
                        case "email":
                            query = query.Where(cus => cus.Email!.Contains(value));
                            break;
                        case "phoneNumber":
                            query = query.Where(cus => cus.PhoneNumber!.Contains(value));
                            break;
                        case "address":
                            query = query.Where(cus => cus.Address!.Contains(value));
                            break;
                        case "name":
                            query = query.Where(cus => cus.FirstName.Contains(value) || cus.LastName.Contains(value));
                            break;
                        default:
                            query = query.Where(cus => EF.Property<string>(cus, filter.Key.CapitalizeWord()) == value);
                            break;
                    }
                }
            }

            return query;
        }

        private IQueryable<Guest> ApplySorting(IQueryable<Guest> query, Dictionary<string, string> sort)
        {
            foreach (var order in sort)
            {
                query =
                    order.Value == "ASC"
                        ? query.OrderBy(cus => EF.Property<object>(cus, order.Key.CapitalizeWord()))
                        : query.OrderByDescending(cus => EF.Property<object>(cus, order.Key.CapitalizeWord()));
            }

            return query;
        }

        public async Task<Guest?> GetGuestById(int guestId)
        {
            return await _dbContext.Guests.Include(g => g.Account).Where(g => g.Account!.IsActive && g.Id == guestId).FirstOrDefaultAsync();
        }

        public async Task<Guest?> GetGuestByAccountId(int accountId)
        {
            return await _dbContext.Guests.SingleOrDefaultAsync(g => g.AccountId == accountId);
        }

        public async Task<Guest?> GetGuestByEmail(string email)
        {
            return await _dbContext
                .Guests.Include(g => g.Account)
                .Where(g => g.Account!.IsActive && g.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<(List<Guest>, int)> GetAllGuests(BaseQueryObject queryObject)
        {
            var query = _dbContext.Guests.Include(cus => cus.Account).AsQueryable();

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

            var guests = await query.ToListAsync();

            return (guests, total);
        }

        public async Task AddGuest(Guest guest)
        {
            _dbContext.Guests.Add(guest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateGuest(Guest guest)
        {
            _dbContext.Guests.Update(guest);
            await _dbContext.SaveChangesAsync();
        }
    }
}
