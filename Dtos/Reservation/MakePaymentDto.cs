using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Enums;

namespace server.Dtos.Reservation
{
    public class MakePaymentDto
    {
        public decimal Amount { get; set; }
        public string Method { get; set; } = PaymentMethod.Cash.ToString();
    }

    public class DepositPaymentDto
    {
        public string Method { get; set; } = PaymentMethod.Cash.ToString();
    }
}
