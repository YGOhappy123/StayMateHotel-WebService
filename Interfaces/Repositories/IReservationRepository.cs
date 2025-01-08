using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;
using server.Models;
using server.Queries;

namespace server.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        Task<List<Room>> FindAvailableRooms(DateTime checkInDate, DateTime checkOutDate, int numberOfGuests);
        Task<(List<Booking>, int)> GetAllBookings(BaseQueryObject queryObject);
        Task<List<Booking>> GetMyBookings(int guestId);
        Task CreateNewBooking(Booking booking);
        Task<Booking?> GetBookingById(int bookingId);
        Task UpdateBooking(Booking booking);
        Task CancelReservationsWithDuplicateRooms(DateTime checkInDate, DateTime checkOutDate, int bookingId, int roomId);
        Task<int> CountBookingsByStatus(BookingStatus status, TimeRangeQueryObject queryObject);
    }
}
