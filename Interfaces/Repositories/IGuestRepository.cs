using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Statistic;
using server.Models;
using server.Queries;

namespace server.Interfaces.Repositories
{
    public interface IGuestRepository
    {
        Task<Guest?> GetGuestById(int guestId);
        Task<Guest?> GetGuestByAccountId(int accountId);
        Task<Guest?> GetGuestByEmail(string email);
        Task<(List<Guest>, int)> GetAllGuests(BaseQueryObject queryObject);
        Task AddGuest(Guest guest);
        Task UpdateGuest(Guest guest);
        Task<int> CountGuestsCreatedInTimeRange(DateTime startTime, DateTime endTime);
        Task<List<GuestWithBookingCount>> GetGuestsWithHighestBookingCountInTimeRange(DateTime startTime, DateTime endTime, int limit);
        Task<List<GuestWithTotalPayment>> GetGuestsWithHighestPaymentAmountInTimeRange(DateTime startTime, DateTime endTime, int limit);
    }
}
