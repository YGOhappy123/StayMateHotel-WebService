using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces.Repositories
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllRooms();
        Task<Room?> GetRoomById(int roomId);
    }
}
