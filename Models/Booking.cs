using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;

namespace server.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime CheckInTime { get; set; } = DateTime.Now;
        public DateTime CheckOutTime { get; set; } = DateTime.Now;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? GuestId { get; set; }
        public Guest? Guest { get; set; }
        public List<Payment> Payments { get; set; } = [];
        public List<BookingRoom> BookingRooms { get; set; } = [];
        public List<BookingService> BookingServices { get; set; } = [];
    }
}
