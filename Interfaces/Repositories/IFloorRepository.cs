using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces.Repositories
{
    public interface IFloorRepository
    {
        Task<List<Floor>> GetAllFloors();
        Task<Floor?> GetFloorById(int floorId);
        Task<Floor> AddFloor(Floor floor);
        Task<bool> UpdateFloor(Floor floor);
        Task<bool> DeleteFloor(int id);
    }
}