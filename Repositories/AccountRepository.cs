using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
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
            return await _dbContext.Accounts.SingleOrDefaultAsync(acc => acc.Username == username);
        }

        public async Task<Account?> GetAccountById(int accountId)
        {
            return await _dbContext.Accounts.SingleOrDefaultAsync(acc => acc.Id == accountId);
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
