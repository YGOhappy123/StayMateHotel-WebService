using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.Reservation
{
    public class OrderBookingServiceDto
    {
        public int? Quantity { get; set; } = 1;
        public int? ServiceId { get; set; }
    }
}
