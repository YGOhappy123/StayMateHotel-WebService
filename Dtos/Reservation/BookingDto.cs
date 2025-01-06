using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Room;
using server.Enums;

namespace server.Dtos.Reservation
{
    public class BookingDto
    {
        public int Id { get; set; }
        public DateTime CheckInTime { get; set; } = DateTime.Now;
        public DateTime CheckOutTime { get; set; } = DateTime.Now;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Status { get; set; } = BookingStatus.Pending.ToString();
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public GuestInfo? Guest { get; set; }
        public List<PaymentInfo>? Payments { get; set; } = [];
        public List<BookingRoomInfo>? BookingRooms { get; set; } = [];
        public List<BookingServiceInfo>? BookingServices { get; set; } = [];
    }

    public class GuestInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class BookingRoomInfo
    {
        public int NumberOfGuests { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public string? RoomNumber { get; set; } = string.Empty;
        public string? Floor { get; set; }
        public string? RoomClass { get; set; }
    }

    public class BookingServiceInfo
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Status { get; set; } = BookingServiceStatus.Pending.ToString();
        public string? Name { get; set; } = string.Empty;
    }

    public class PaymentInfo
    {
        public decimal Amount { get; set; }
        public DateTime PaymentTime { get; set; } = DateTime.Now;
        public string Method { get; set; } = PaymentMethod.Cash.ToString();
    }
}
