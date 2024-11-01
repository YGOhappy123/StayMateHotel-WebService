using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces.Repositories
{
    public interface IGuestRepository
    {
        Task<Guest?> GetGuestById(int guestId);
        Task<Guest?> GetGuestByAccountId(int accountId);
        Task AddGuest(Guest guest);
        Task UpdateGuest(Guest guest);
    }
}
