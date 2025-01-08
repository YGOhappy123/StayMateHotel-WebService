using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;

namespace server.Dtos.Reservation
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentTime { get; set; } = DateTime.Now;
        public string Method { get; set; } = PaymentMethod.Cash.ToString();
        public int BookingId { get; set; }
    }
}
