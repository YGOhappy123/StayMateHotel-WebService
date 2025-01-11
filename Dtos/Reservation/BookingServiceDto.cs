using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;

namespace server.Dtos.Reservation
{
    public class BookingServiceDto
    {
        public int Id { get; set; }
        public int? Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Status { get; set; } = BookingServiceStatus.Pending.ToString();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public BookingInfo? Booking { get; set; }
        public ServiceInfo? Service { get; set; }
    }

    public class BookingInfo
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public GuestInfo? Guest { get; set; }
    }

    public class ServiceInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
