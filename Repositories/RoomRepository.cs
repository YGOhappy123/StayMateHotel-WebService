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
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public RoomRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Room>> GetAllRooms()
        {
            return await _dbContext.Rooms.ToListAsync();
        }

        public async Task<Room?> GetRoomById(int roomId)
        {
            return await _dbContext.Rooms.SingleOrDefaultAsync(r => r.Id == roomId);
        }
    }
}
