using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Reservation;
using server.Dtos.Response;
using server.Models;
using server.Queries;

namespace server.Interfaces.Services
{
    public interface IReservationService
    {
        Task<ServiceResponse<List<List<Room>>>> FindAvailableRooms(BaseQueryObject queryObject);
        Task<ServiceResponse<int>> MakeNewBooking(MakeBookingDto makeBookingDto, int guestId);
    }
}
