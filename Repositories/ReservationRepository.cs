using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Enums;
using server.Interfaces.Repositories;
using server.Models;
using server.Queries;

namespace server.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IRoomRepository _roomRepo;

        public ReservationRepository(ApplicationDBContext context, IRoomRepository roomRepo)
        {
            _dbContext = context;
            _roomRepo = roomRepo;
        }

        public async Task<List<Room>> FindAvailableRooms(DateTime checkInDate, DateTime checkOutDate, int numberOfGuests)
        {
            var overlappingBookingRoomIds = await _dbContext
                .Bookings.Where(bk =>
                    bk.Status != BookingStatus.Pending
                    && bk.Status != BookingStatus.Cancelled
                    && bk.CheckOutTime >= checkInDate
                    && bk.CheckInTime <= checkOutDate
                )
                .SelectMany(bk => bk.BookingRooms.Select(br => br.RoomId))
                .Distinct()
                .ToListAsync();

            var roomIds = await _dbContext
                .Rooms.Where(rm =>
                    !overlappingBookingRoomIds.Contains(rm.Id)
                    && rm.RoomClass!.Capacity >= numberOfGuests
                    && (rm.Status == RoomStatus.Available || rm.Status == RoomStatus.UnderCleaning)
                )
                .OrderBy(rm => rm.RoomClass!.BasePrice)
                .Select(rm => rm.Id)
                .ToListAsync();

            List<Room> result = [];
            foreach (var roomId in roomIds)
            {
                var room = await _roomRepo.GetRoomById(roomId);
                if (room != null)
                {
                    result.Add(room);
                }
            }

            return result;
        }

        public async Task CreateNewBooking(Booking booking)
        {
            _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync();
        }
    }
}
