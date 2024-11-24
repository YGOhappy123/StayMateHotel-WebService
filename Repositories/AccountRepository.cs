using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Enums;
using server.Interfaces.Repositories;
using server.Models;

namespace server.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public AccountRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        public async Task<Account?> GetAccountByUsername(string username)
        {
            return await _dbContext.Accounts.SingleOrDefaultAsync(acc => acc.IsActive && acc.Username == username);
        }

        public async Task<Account?> GetAccountById(int accountId)
        {
            return await _dbContext.Accounts.SingleOrDefaultAsync(acc => acc.IsActive && acc.Id == accountId);
        }

        public async Task<Account?> GetGuestAccountByEmail(string email)
        {
            return await _dbContext
                .Accounts.Where(acc => acc.IsActive && acc.Guest != null && acc.Guest.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<Account?> GetAccountByUserIdAndRole(int userId, string role)
        {
            if (role == UserRole.Guest.ToString())
            {
                return await _dbContext
                    .Accounts.Where(acc => acc.IsActive && acc.Guest != null && acc.Guest.Id == userId)
                    .FirstOrDefaultAsync();
            }
            else
            {
                return await _dbContext
                    .Accounts.Where(acc => acc.IsActive && acc.Admin != null && acc.Admin.Id == userId)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task AddAccount(Account account)
        {
            _dbContext.Accounts.Add(account);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAccount(Account account)
        {
            _dbContext.Accounts.Update(account);
            await _dbContext.SaveChangesAsync();
        }
    }
}
