using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;
using server.Queries;

namespace server.Interfaces.Repositories
{
    public interface IRoomRepository
    {
        Task<(List<Room>, int)> GetAllRooms(BaseQueryObject queryObject);
        Task<Room?> GetRoomById(int roomId);
        Task<Room?> GetRoomByRoomNumber(string roomNumber);
        Task CreateNewRoom(Room room);
        Task UpdateRoom(Room room);
        Task DeleteRoom(Room room);
        Task<int> CountBookedTimes(int roomId);
        Task<bool> CheckIfRoomIsBooked(int roomId);
        Task DeleteOldImagesOfRoom(int roomId);
        Task<int> GetRoomStatisticInTimeRange(DateTime startTime, DateTime endTime, int roomId);
    }
}
