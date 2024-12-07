using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Interfaces.Repositories;
using server.Interfaces.Services;
using server.Models;

namespace server.Services
{
    public class RoomClassService : IRoomClassService
    {
        private readonly IRoomClassRepository _repository;

        public RoomClassService(IRoomClassRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RoomClass>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<RoomClass?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(RoomClass roomClass)
        {
            await _repository.AddAsync(roomClass);
        }

        public async Task UpdateAsync(RoomClass roomClass)
        {
            await _repository.UpdateAsync(roomClass);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}