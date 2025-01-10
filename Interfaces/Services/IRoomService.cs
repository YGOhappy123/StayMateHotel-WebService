using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Response;
using server.Dtos.Room;
using server.Models;
using server.Queries;

namespace server.Interfaces.Services
{
    public interface IRoomService
    {
        Task<ServiceResponse<List<Room>>> GetAllRooms(BaseQueryObject queryObject);
        Task<ServiceResponse<object>> GetRoomById(int roomId);
        Task<ServiceResponse> CreateNewRoom(CreateUpdateRoomDto createRoomDto, int adminId);
        Task<ServiceResponse> UpdateRoom(int roomId, CreateUpdateRoomDto updateRoomDto);
        Task<ServiceResponse> DeleteRoom(int roomId);
        Task<ServiceResponse> ToggleMaintenance(int roomId);
        Task<ServiceResponse> MarkCleaningDone(int roomId);
    }
}
