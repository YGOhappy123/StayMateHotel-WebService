using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;
using server.Queries;

namespace server.Interfaces.Repositories
{
    public interface IAdminRepository
    {
        Task<Admin?> GetAdminById(int adminId);
        Task<Admin?> GetAdminByAccountId(int accountId);
        Task<Admin?> GetAdminByEmail(string email);
        Task<Admin?> GetAdminByIdIncludeInactive(int adminId);
        Task<Admin?> GetAdminByEmailIncludeInactive(string email);
        Task<Admin?> GetAdminByPhoneNumber(string phoneNumber);
        Task<Admin?> GetAdminByPhoneNumberIncludeInactive(string phoneNumber);
        Task<(List<Admin>, int)> GetAllAdmins(BaseQueryObject queryObject);
        Task AddAdmin(Admin admin);
        Task UpdateAdmin(Admin admin);
    }
}
