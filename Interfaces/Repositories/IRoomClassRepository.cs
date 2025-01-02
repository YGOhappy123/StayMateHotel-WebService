using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;
using server.Queries;

namespace server.Interfaces.Repositories
{
    public interface IRoomClassRepository
    {
        Task<(List<RoomClass>, int)> GetAllRoomClasses(BaseQueryObject queryObject);
        Task<RoomClass?> GetRoomClassById(int roomClassId);
        Task<RoomClass?> GetRoomClassByName(string roomClassName);
        Task CreateNewRoomClass(RoomClass roomClass);
        Task UpdateRoomClass(RoomClass roomClass);
        Task DeleteRoomClass(RoomClass roomClass);
        Task<int> CountRoomsInRoomClass(int roomId);
        Task DeleteFeatureOfRoomClass(int roomClassId);
    }
}
