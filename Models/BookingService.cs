using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class BookingService
    {
        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }
        public int? Quantity { get; set; } = 1;
    }
}
