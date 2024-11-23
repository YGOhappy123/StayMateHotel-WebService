using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Guest : AppUser
    {
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public List<Booking> Bookings { get; set; } = [];
    }
}
