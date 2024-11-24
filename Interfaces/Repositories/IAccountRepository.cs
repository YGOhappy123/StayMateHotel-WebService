using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountByUsername(string username);
        Task<Account?> GetAccountById(int accountId);
        Task<Account?> GetGuestAccountByEmail(string email);
        Task<Account?> GetAccountByUserIdAndRole(int userId, string role);
        Task AddAccount(Account account);
        Task UpdateAccount(Account account);
    }
}
