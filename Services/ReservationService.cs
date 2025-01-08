using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using server.Dtos.Reservation;
using server.Dtos.Response;
using server.Enums;
using server.Interfaces.Repositories;
using server.Interfaces.Services;
using server.Models;
using server.Queries;
using server.Utilities;

namespace server.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly IPaymentRepository _paymentRepo;

        public ReservationService(IReservationRepository reservationRepo, IRoomRepository roomRepo, IPaymentRepository paymentRepo)
        {
            _reservationRepo = reservationRepo;
            _roomRepo = roomRepo;
            _paymentRepo = paymentRepo;
        }

        public async Task<ServiceResponse<List<List<Room>>>> FindAvailableRooms(BaseQueryObject queryObject)
        {
            var parsedFilter = JsonSerializer.Deserialize<Dictionary<string, object>>(queryObject.Filter!);

            List<List<Room>> results = [];
            if (parsedFilter!["guests"] is JsonElement element && element.ValueKind == JsonValueKind.Array)
            {
                foreach (var numberOfGuests in element.EnumerateArray())
                {
                    var roomList = await _reservationRepo.FindAvailableRooms(
                        DateTime.Parse(parsedFilter!["checkInDate"].ToString()!),
                        DateTime.Parse(parsedFilter!["checkOutDate"].ToString()!),
                        numberOfGuests.GetInt32()
                    );
                    results.Add(roomList);
                }
            }

            return new ServiceResponse<List<List<Room>>>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = results,
            };
        }

        public async Task<ServiceResponse<List<Booking>>> GetAllBookings(BaseQueryObject queryObject)
        {
            var (bookings, total) = await _reservationRepo.GetAllBookings(queryObject);

            return new ServiceResponse<List<Booking>>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = bookings,
                Total = total,
                Took = bookings.Count,
            };
        }

        public async Task<ServiceResponse<List<Booking>>> GetMyBookings(int guestId)
        {
            var bookings = await _reservationRepo.GetMyBookings(guestId);

            return new ServiceResponse<List<Booking>>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = bookings,
            };
        }

        public async Task<ServiceResponse<int>> MakeNewBooking(MakeBookingDto makeBookingDto, int guestId)
        {
            var newBooking = new Booking
            {
                CheckInTime = makeBookingDto.CheckInTime,
                CheckOutTime = makeBookingDto.CheckOutTime,
                Email = makeBookingDto.Email,
                PhoneNumber = makeBookingDto.PhoneNumber,
                TotalAmount = 0,
                Status = BookingStatus.Pending,
                GuestId = guestId,
                BookingRooms = [],
            };

            TimeSpan difference = makeBookingDto.CheckOutTime - makeBookingDto.CheckInTime;
            int dayDiff = difference.Days;

            foreach (var room in makeBookingDto.BookingRooms)
            {
                var roomInfo = await _roomRepo.GetRoomById(room.RoomId);
                if (roomInfo != null)
                {
                    newBooking.TotalAmount += roomInfo.RoomClass!.BasePrice * dayDiff;
                    newBooking.BookingRooms.Add(
                        new BookingRoom
                        {
                            NumberOfGuests = room.NumberOfGuests,
                            UnitPrice = roomInfo.RoomClass!.BasePrice,
                            RoomId = room.RoomId,
                        }
                    );
                }
            }

            await _reservationRepo.CreateNewBooking(newBooking);

            return new ServiceResponse<int>
            {
                Status = ResStatusCode.CREATED,
                Success = true,
                Message = SuccessMessage.MAKE_RESERVATION_SUCCESSFULLY,
                Data = newBooking.Id,
            };
        }

        public async Task<ServiceResponse> AcceptBooking(int bookingId)
        {
            var booking = await _reservationRepo.GetBookingById(bookingId);
            if (booking == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.BOOKING_NOT_FOUND,
                };
            }

            if (booking.Status != BookingStatus.Pending)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.CANNOT_UPDATE_BOOKING_WITH_THIS_STATUS,
                };
            }

            booking.Status = BookingStatus.Confirmed;
            await _reservationRepo.UpdateBooking(booking);

            foreach (var bkr in booking.BookingRooms)
            {
                await _reservationRepo.CancelReservationsWithDuplicateRooms(
                    booking.CheckInTime,
                    booking.CheckOutTime,
                    bookingId,
                    bkr!.Room!.Id
                );
            }

            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.UPDATE_BOOKING_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> CancelBooking(int bookingId, int authUserId, string authUserRole)
        {
            var booking = await _reservationRepo.GetBookingById(bookingId);
            if (booking == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.BOOKING_NOT_FOUND,
                };
            }

            if (authUserRole == UserRole.Guest.ToString())
            {
                if (booking.GuestId != authUserId)
                {
                    return new ServiceResponse
                    {
                        Status = ResStatusCode.FORBIDDEN,
                        Success = false,
                        Message = ErrorMessage.NO_PERMISSION,
                    };
                }

                if (
                    (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Confirmed)
                    || booking.Status == BookingStatus.Cancelled
                )
                {
                    return new ServiceResponse
                    {
                        Status = ResStatusCode.NOT_FOUND,
                        Success = false,
                        Message = ErrorMessage.CANNOT_UPDATE_BOOKING_WITH_THIS_STATUS,
                    };
                }
            }
            else
            {
                var totalPayments = await _paymentRepo.GetBookingToTalPayments(bookingId);
                var today = DateTime.Now.Date;

                bool isPending = booking.Status == BookingStatus.Pending;
                bool isCancelled = booking.Status == BookingStatus.Cancelled;
                bool isConfirmedWithoutDeposit = booking.Status == BookingStatus.Confirmed && totalPayments == 0;
                bool isAfterCheckInWithoutDeposit = today > booking.CheckInTime.Date && totalPayments == 0;

                if ((!isPending && !isConfirmedWithoutDeposit && !isAfterCheckInWithoutDeposit) || isCancelled)
                {
                    return new ServiceResponse
                    {
                        Status = ResStatusCode.NOT_FOUND,
                        Success = false,
                        Message = ErrorMessage.CANNOT_UPDATE_BOOKING_WITH_THIS_STATUS,
                    };
                }
            }

            booking.Status = BookingStatus.Cancelled;
            await _reservationRepo.UpdateBooking(booking);

            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.UPDATE_BOOKING_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> CheckInBooking(int bookingId)
        {
            var booking = await _reservationRepo.GetBookingById(bookingId);
            if (booking == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.BOOKING_NOT_FOUND,
                };
            }

            if (booking.Status != BookingStatus.Confirmed)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.CANNOT_UPDATE_BOOKING_WITH_THIS_STATUS,
                };
            }

            foreach (var bkr in booking.BookingRooms)
            {
                if (bkr == null || bkr.Room == null || bkr.Room.Status == RoomStatus.UnderCleaning)
                {
                    return new ServiceResponse
                    {
                        Status = ResStatusCode.NOT_FOUND,
                        Success = false,
                        Message = ErrorMessage.PLEASE_CLEAN_THE_ROOMS_FIRST,
                    };
                }
            }

            var today = DateTime.Now.Date;
            var totalPayments = await _paymentRepo.GetBookingToTalPayments(bookingId);

            if (today < booking.CheckInTime.Date || totalPayments == 0)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.BAD_REQUEST,
                    Success = false,
                    Message = ErrorMessage.INVALID_DATE_OR_NO_DEPOSIT_TRACKED,
                };
            }

            booking.Status = BookingStatus.CheckedIn;
            await _reservationRepo.UpdateBooking(booking);

            foreach (var bkr in booking.BookingRooms)
            {
                var room = bkr!.Room;
                room!.Status = RoomStatus.Occupied;
                await _roomRepo.UpdateRoom(room);
            }

            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.UPDATE_BOOKING_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> CheckOutBooking(int bookingId)
        {
            var booking = await _reservationRepo.GetBookingById(bookingId);
            if (booking == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.BOOKING_NOT_FOUND,
                };
            }

            if (booking.Status != BookingStatus.CheckedIn)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.CANNOT_UPDATE_BOOKING_WITH_THIS_STATUS,
                };
            }

            var today = DateTime.Now.Date;
            if (today < booking.CheckOutTime.Date)
            {
                int totalDaysBooked = (booking.CheckOutTime - booking.CheckInTime).Days;
                int totalDaysStayed = (today - booking.CheckInTime).Days;

                if (totalDaysStayed == 0)
                {
                    totalDaysStayed = 1;
                }

                decimal totalUnitPrice = booking.BookingRooms.Sum(bkr => bkr.UnitPrice);
                booking.TotalAmount -= (totalDaysBooked - totalDaysStayed) * totalUnitPrice;
            }

            booking.Status = BookingStatus.CheckedOut;
            booking.CheckOutTime = DateTime.Now;
            await _reservationRepo.UpdateBooking(booking);
            await CheckPaymentDone(booking);

            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.UPDATE_BOOKING_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> DepositBooking(int bookingId, DepositPaymentDto paymentDto)
        {
            var booking = await _reservationRepo.GetBookingById(bookingId);
            if (booking == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.BOOKING_NOT_FOUND,
                };
            }

            if (booking.Status != BookingStatus.Confirmed)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.CANNOT_UPDATE_BOOKING_WITH_THIS_STATUS,
                };
            }

            var newPayment = new Payment
            {
                Amount = Math.Ceiling(booking.TotalAmount / 10000) * 1000,
                Method = Enum.Parse<PaymentMethod>(paymentDto.Method),
                BookingId = bookingId,
            };
            await _paymentRepo.MakeNewPayment(newPayment);

            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.PAYMENT_TRACKED_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> MakePaymentBooking(int bookingId, MakePaymentDto paymentDto)
        {
            var booking = await _reservationRepo.GetBookingById(bookingId);
            if (booking == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.BOOKING_NOT_FOUND,
                };
            }

            if (booking.Status != BookingStatus.CheckedOut)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.CANNOT_UPDATE_BOOKING_WITH_THIS_STATUS,
                };
            }

            var newPayment = new Payment
            {
                Amount = paymentDto.Amount,
                Method = Enum.Parse<PaymentMethod>(paymentDto.Method),
                BookingId = bookingId,
            };
            await _paymentRepo.MakeNewPayment(newPayment);
            await CheckPaymentDone(booking);

            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.PAYMENT_TRACKED_SUCCESSFULLY,
            };
        }

        private async Task CheckPaymentDone(Booking booking)
        {
            var totalPayments = await _paymentRepo.GetBookingToTalPayments(booking.Id);
            if (totalPayments >= booking.TotalAmount)
            {
                booking.Status = BookingStatus.PaymentDone;
                await _reservationRepo.UpdateBooking(booking);

                foreach (var bkr in booking.BookingRooms)
                {
                    var room = bkr!.Room;
                    room!.Status = RoomStatus.UnderCleaning;
                    await _roomRepo.UpdateRoom(room);
                }
            }
        }

        public async Task<ServiceResponse<object>> CountBookingsByStatus(TimeRangeQueryObject queryObject)
        {
            var statusCounts = new Dictionary<string, int>();

            foreach (BookingStatus status in Enum.GetValues(typeof(BookingStatus)))
            {
                var count = await _reservationRepo.CountBookingsByStatus(status, queryObject);
                statusCounts[status.ToString()] = count;
            }

            return new ServiceResponse<object>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = statusCounts,
            };
        }

        public async Task<ServiceResponse<List<Payment>>> GetAllTransactions(BaseQueryObject queryObject)
        {
            var (payments, total) = await _paymentRepo.GetAllPayments(queryObject);

            return new ServiceResponse<List<Payment>>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = payments,
                Total = total,
                Took = payments.Count,
            };
        }
    }
}
