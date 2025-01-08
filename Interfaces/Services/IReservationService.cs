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
        Task<ServiceResponse<List<Booking>>> GetAllBookings(BaseQueryObject queryObject);
        Task<ServiceResponse<List<Booking>>> GetMyBookings(int guestId);
        Task<ServiceResponse<int>> MakeNewBooking(MakeBookingDto makeBookingDto, int guestId);
        Task<ServiceResponse> AcceptBooking(int bookingId);
        Task<ServiceResponse> CancelBooking(int bookingId, int authUserId, string authUserRole);
        Task<ServiceResponse> CheckInBooking(int bookingId);
        Task<ServiceResponse> CheckOutBooking(int bookingId);
        Task<ServiceResponse> DepositBooking(int bookingId, DepositPaymentDto paymentDto);
        Task<ServiceResponse> MakePaymentBooking(int bookingId, MakePaymentDto paymentDto);
        Task<ServiceResponse<object>> CountBookingsByStatus(TimeRangeQueryObject queryObject);
        Task<ServiceResponse<List<Payment>>> GetAllTransactions(BaseQueryObject queryObject);
    }
}
