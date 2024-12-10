using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces.Repositories;
using server.Models;
using System.Threading.Tasks;

namespace server.Repositories
{
    public class RoomClassFeatureRepository : IRoomClassFeatureRepository
    {
        private readonly ApplicationDBContext _context;

        public RoomClassFeatureRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        // Phương thức xóa RoomClassFeature
        public async Task DeleteRoomClassFeature(RoomClassFeature roomClassFeature)
        {
            _context.RoomClassFeatures.Remove(roomClassFeature);
            await _context.SaveChangesAsync();
        }
    }
}
