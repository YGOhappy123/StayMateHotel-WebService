using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;
using server.Queries;

namespace server.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task<(List<Payment>, int)> GetAllPayments(BaseQueryObject queryObject);
        Task<decimal> GetBookingToTalPayments(int bookingId);
        Task MakeNewPayment(Payment payment);
    }
}
