using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;
using server.Models;
using server.Queries;

namespace server.Interfaces.Repositories
{
    public interface IBookingServiceRepository
    {
        Task<(List<BookingService>, int)> GetAllBookingServices(BaseQueryObject queryObject);
        Task<BookingService?> GetBookingServiceById(int bookingServiceId);
        Task CreateNewBookingService(BookingService bookingService);
        Task UpdateBookingService(BookingService bookingService);
        Task<int> CountBookingsByStatus(BookingServiceStatus status, TimeRangeQueryObject queryObject);
    }
}
