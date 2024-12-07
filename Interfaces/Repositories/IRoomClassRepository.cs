using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces.Repositories
{
    public interface IRoomClassRepository
    {
        Task<IEnumerable<RoomClass>> GetAllAsync();
        Task<RoomClass?> GetByIdAsync(int id);
        Task AddAsync(RoomClass roomClass);
        Task UpdateAsync(RoomClass roomClass);
        Task DeleteAsync(int id);
    }
}