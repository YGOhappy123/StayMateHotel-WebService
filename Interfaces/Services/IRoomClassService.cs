using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Response;
using server.Dtos.RoomClass;
using server.Models;
using server.Queries;

namespace server.Interfaces.Services
{
    public interface IRoomClassService
    {
        Task<ServiceResponse<List<RoomClass>>> GetAllRoomClasses(BaseQueryObject queryObject);
        Task<ServiceResponse<RoomClass>> GetRoomClassById(int roomClassId);
        Task<ServiceResponse> CreateNewRoomClass(CreateUpdateRoomClassDto createRoomClassDto, int adminId);
        Task<ServiceResponse> UpdateRoomClass(int roomClassId, CreateUpdateRoomClassDto updateRoomClassDto);
        Task<ServiceResponse> DeleteRoomClass(int roomClassId);
    }
}
