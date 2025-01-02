using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.Reservation
{
    public class MakeBookingDto
    {
        public DateTime CheckInTime { get; set; } = DateTime.Now;
        public DateTime CheckOutTime { get; set; } = DateTime.Now;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<BookingRoomDto> BookingRooms { get; set; } = [];
    }

    public class BookingRoomDto
    {
        public int NumberOfGuests { get; set; } = 1;
        public int RoomId { get; set; }
    }
}
