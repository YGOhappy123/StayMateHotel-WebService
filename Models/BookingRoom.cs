using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class BookingRoom
    {
        public int NumberOfGuests { get; set; } = 1;

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }
        public int? RoomId { get; set; }
        public Room? Room { get; set; }
    }
}
