using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces
{
    public interface IRoomService
    {
        List<Room> GetAllRooms();
        Room? GetRoomById(int roomId);
    }
}
