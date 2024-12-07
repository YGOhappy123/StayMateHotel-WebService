using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces.Services
{
    public interface IFloorService
    {
        Task<List<Floor>> GetAllFloors();
        Task<Floor?> GetFloorById(int id);
        Task<Floor> AddFloor(Floor floor);
        Task<bool> UpdateFloor(Floor floor);
        Task<bool> DeleteFloor(int id);
    }
}