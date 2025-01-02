using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;
using server.Queries;

namespace server.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        Task<List<Room>> FindAvailableRooms(DateTime checkInDate, DateTime checkOutDate, int numberOfGuests);
        Task CreateNewBooking(Booking booking);
    }
}
