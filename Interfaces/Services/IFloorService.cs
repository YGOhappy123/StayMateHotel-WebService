using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Floor;
using server.Dtos.Response;
using server.Models;
using server.Queries;

namespace server.Interfaces.Services
{
    public interface IFloorService
    {
        Task<ServiceResponse<List<Floor>>> GetAllFloors(BaseQueryObject queryObject);
        Task<ServiceResponse<Floor>> GetFloorById(int orderId);
        Task<ServiceResponse> CreateNewFloor(CreateUpdateFloorDto createFloorDto, int adminId);
        Task<ServiceResponse> UpdateFloor(int floorId, CreateUpdateFloorDto updateFloorDto);
        Task<ServiceResponse> DeleteFloor(int floorId);
    }
}
