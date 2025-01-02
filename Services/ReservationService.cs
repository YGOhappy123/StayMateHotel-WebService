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

        public ReservationService(IReservationRepository reservationRepo, IRoomRepository roomRepo)
        {
            _reservationRepo = reservationRepo;
            _roomRepo = roomRepo;
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
                    newBooking.BookingRooms.Add(new BookingRoom { NumberOfGuests = room.NumberOfGuests, RoomId = room.RoomId });
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
    }
}
