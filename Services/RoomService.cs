using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Data;
using server.Interfaces;
using server.Models;

namespace server.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDBContext _dbContext;

        public RoomService(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        public List<Room> GetAllRooms()
        {
            return _dbContext.Rooms.ToList();
        }

        public Room? GetRoomById(int roomId)
        {
            return _dbContext.Rooms.SingleOrDefault(x => x.Id == roomId);
        }
    }
}
