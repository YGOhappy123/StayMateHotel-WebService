using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;

namespace server.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public DateTime PaymentTime { get; set; } = DateTime.Now;
        public PaymentMethod Method { get; set; } = PaymentMethod.Cash;
        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }
    }
}
