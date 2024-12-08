using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;
using server.Queries;

namespace server.Interfaces.Repositories
{
    public interface IFloorRepository
    {
        Task<(List<Floor>, int)> GetAllFloors(BaseQueryObject queryObject);
        Task<Floor?> GetFloorById(int floorId);
        Task<List<Floor>> GetFloorsByFloorNumber(string floorNumber);
        Task AddFloor(Floor floor);
        Task UpdateFloor(Floor floor);
        Task DeleteFloor(Floor floor);
    }
}