using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Interfaces.Repositories;
using server.Interfaces.Services;
using server.Models;
using server.Repositories;

namespace server.Services
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _floorRepository;

        public FloorService(IFloorRepository floorRepository)
        {
            _floorRepository = floorRepository;
        }

        public async Task<List<Floor>> GetAllFloors()
        {
            var floors = await _floorRepository.GetAllFloors();
            return floors;
        }

        public async Task<Floor?> GetFloorById(int id)
        {
            var floor = await _floorRepository.GetFloorById(id);

            if (floor == null)
            {
                throw new KeyNotFoundException($"Floor with ID {id} not found.");
            }

            return floor;
        }

        public async Task<Floor> AddFloor(Floor floor)
        {
            if (floor == null)
            {
                throw new ArgumentNullException(nameof(floor), "Floor cannot be null.");
            }

            var addedFloor = await _floorRepository.AddFloor(floor);
            return addedFloor;
        }

        public async Task<bool> UpdateFloor(Floor floor)
        {
            if (floor == null)
            {
                throw new ArgumentNullException(nameof(floor), "Floor cannot be null.");
            }

            var result = await _floorRepository.UpdateFloor(floor);
            return result;
        }

        public async Task<bool> DeleteFloor(int id)
        {
            var result = await _floorRepository.DeleteFloor(id);
            return result;
        }

    }
}