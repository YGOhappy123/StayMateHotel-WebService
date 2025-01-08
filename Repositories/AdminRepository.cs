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
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public AdminRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        private IQueryable<Admin> ApplyFilters(IQueryable<Admin> query, Dictionary<string, object> filters)
        {
            foreach (var filter in filters)
            {
                string value = filter.Value.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(value))
                {
                    switch (filter.Key)
                    {
                        case "startTime":
                            query = query.Where(ad => ad.CreatedAt >= DateTime.Parse(value));
                            break;
                        case "endTime":
                            query = query.Where(f => f.CreatedAt <= TimestampHandler.GetEndOfTimeByType(DateTime.Parse(value), "daily"));
                            break;
                        case "email":
                            query = query.Where(ad => ad.Email!.Contains(value));
                            break;
                        case "phoneNumber":
                            query = query.Where(ad => ad.PhoneNumber!.Contains(value));
                            break;
                        case "name":
                            query = query.Where(ad => ad.FirstName.Contains(value) || ad.LastName.Contains(value));
                            break;
                        default:
                            query = query.Where(ad => EF.Property<string>(ad, filter.Key.CapitalizeWord()) == value);
                            break;
                    }
                }
            }

            return query;
        }

        private IQueryable<Admin> ApplySorting(IQueryable<Admin> query, Dictionary<string, string> sort)
        {
            foreach (var order in sort)
            {
                query =
                    order.Value == "ASC"
                        ? query.OrderBy(ad => EF.Property<object>(ad, order.Key.CapitalizeWord()))
                        : query.OrderByDescending(ad => EF.Property<object>(ad, order.Key.CapitalizeWord()));
            }

            return query;
        }

        public async Task<Admin?> GetAdminById(int adminId)
        {
            return await _dbContext
                .Admins.Include(ad => ad.Account)
                .Where(ad => ad.Account!.IsActive && ad.Id == adminId)
                .FirstOrDefaultAsync();
        }

        public async Task<Admin?> GetAdminByIdIncludeInactive(int adminId)
        {
            return await _dbContext.Admins.Include(ad => ad.Account).Where(ad => ad.Id == adminId).FirstOrDefaultAsync();
        }

        public async Task<Admin?> GetAdminByAccountId(int accountId)
        {
            return await _dbContext.Admins.SingleOrDefaultAsync(ad => ad.AccountId == accountId);
        }

        public async Task<Admin?> GetAdminByEmail(string email)
        {
            return await _dbContext
                .Admins.Include(ad => ad.Account)
                .Where(ad => ad.Account!.IsActive && ad.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<Admin?> GetAdminByEmailIncludeInactive(string email)
        {
            return await _dbContext.Admins.Include(ad => ad.Account).Where(ad => ad.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Admin?> GetAdminByPhoneNumber(string phoneNumber)
        {
            return await _dbContext
                .Admins.Include(ad => ad.Account)
                .Where(ad => ad.Account!.IsActive && ad.PhoneNumber == phoneNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<Admin?> GetAdminByPhoneNumberIncludeInactive(string phoneNumber)
        {
            return await _dbContext.Admins.Include(ad => ad.Account).Where(ad => ad.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
        }

        public async Task<(List<Admin>, int)> GetAllAdmins(BaseQueryObject queryObject)
        {
            var query = _dbContext.Admins.Include(ad => ad.Account).Include(ad => ad.CreatedBy).AsQueryable();

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

            var customers = await query.ToListAsync();

            return (customers, total);
        }

        public async Task AddAdmin(Admin admin)
        {
            _dbContext.Admins.Add(admin);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAdmin(Admin admin)
        {
            _dbContext.Admins.Update(admin);
            await _dbContext.SaveChangesAsync();
        }
    }
}
