using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces.Repositories;
using server.Models;

namespace server.Repositories
{
    public class FloorRepository : IFloorRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public FloorRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Floor>> GetAllFloors()
        {
            return await _dbContext.Floors.ToListAsync();
        }

        public async Task<Floor?> GetFloorById(int floorId)
        {
            return await _dbContext.Floors.SingleOrDefaultAsync(f => f.Id == floorId);
        }

        public async Task<Floor> AddFloor(Floor floor)
        {
            await _dbContext.Floors.AddAsync(floor);
            await _dbContext.SaveChangesAsync();
            return floor;
        }

        public async Task<bool> UpdateFloor(Floor floor)
        {
            var existingFloor = await _dbContext.Floors.FindAsync(floor.Id);
            if (existingFloor == null)
            {
                return false;
            }

            existingFloor.FloorNumber = floor.FloorNumber;
            existingFloor.CreatedAt = floor.CreatedAt;
            existingFloor.CreatedById = floor.CreatedById;

            _dbContext.Floors.Update(existingFloor);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFloor(int id)
        {
            var floor = await _dbContext.Floors.FindAsync(id);
            if (floor == null)
            {
                return false;
            }

            _dbContext.Floors.Remove(floor);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}