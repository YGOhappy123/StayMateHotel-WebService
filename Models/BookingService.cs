using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;

namespace server.Models
{
    public class BookingService
    {
        public int? Quantity { get; set; } = 1;

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        public BookingServiceStatus Status { get; set; } = BookingServiceStatus.Pending;
        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
