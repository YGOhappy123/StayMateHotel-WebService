using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces.Repositories;
using server.Models;

namespace server.Repositories
{
    public class RoomClassFeatureRepository : IRoomClassFeatureRepository
    {
        private readonly ApplicationDBContext _context;

        public RoomClassFeatureRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        //public async Task<List<RoomClassFeature>> GetAllRoomClassFeaturesAsync()
        //{
        //    return await _context.RoomClassFeatures
        //        .Include(rcf => rcf.Feature)
        //        .Include(rcf => rcf.RoomClass) // Load RoomClass để tránh null
        //        .ToListAsync();
        //}
        // Phương thức xóa RoomClassFeature
        public async Task DeleteRoomClassFeature(RoomClassFeature roomClassFeature)
        {
            _context.RoomClassFeatures.Remove(roomClassFeature);
            await _context.SaveChangesAsync();
        }
    }
}
