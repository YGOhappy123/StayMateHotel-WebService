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
    public class RoomClassRepository : IRoomClassRepository
    {
        private readonly ApplicationDBContext _context;

        // Constructor để nhận vào AppDbContext
        public RoomClassRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        // Lấy tất cả RoomClass
        public async Task<IEnumerable<RoomClass>> GetAllAsync()
        {
            return await _context.RoomClasses
                                 .Include(rc => rc.RoomClassFeatures)  // Bao gồm các RoomClassFeatures liên quan
                                 .ToListAsync();
        }

        // Lấy RoomClass theo Id
        public async Task<RoomClass?> GetByIdAsync(int id)
        {
            return await _context.RoomClasses
                                 .Include(rc => rc.RoomClassFeatures)  // Bao gồm các RoomClassFeatures liên quan
                                 .FirstOrDefaultAsync(rc => rc.Id == id);  // Tìm theo Id
        }

        // Thêm RoomClass mới
        public async Task AddAsync(RoomClass roomClass)
        {
            await _context.RoomClasses.AddAsync(roomClass);  // Thêm vào DbContext
            await _context.SaveChangesAsync();  // Lưu thay đổi vào cơ sở dữ liệu
        }

        // Cập nhật RoomClass
        public async Task UpdateAsync(RoomClass roomClass)
        {
            _context.RoomClasses.Update(roomClass);  // Cập nhật phòng học trong DbContext
            await _context.SaveChangesAsync();  // Lưu thay đổi vào cơ sở dữ liệu
        }

        // Xóa RoomClass theo Id
        public async Task DeleteAsync(int id)
        {
            var roomClass = await GetByIdAsync(id);  // Tìm RoomClass theo Id
            if (roomClass != null)
            {
                _context.RoomClasses.Remove(roomClass);  // Xóa RoomClass từ DbContext
                await _context.SaveChangesAsync();  // Lưu thay đổi vào cơ sở dữ liệu
            }
        }
    }
}
