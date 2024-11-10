using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces.Repositories;
using server.Models;

namespace server.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public GuestRepository(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        public async Task<Guest?> GetGuestById(int guestId)
        {
            return await _dbContext.Guests.SingleOrDefaultAsync(g => g.Id == guestId);
        }

        public async Task<Guest?> GetGuestByAccountId(int accountId)
        {
            return await _dbContext.Guests.SingleOrDefaultAsync(g => g.AccountId == accountId);
        }

        public async Task<Guest?> GetGuestByEmail(string email, bool isAccountIncluded)
        {
            if (isAccountIncluded)
            {
                return await _dbContext.Guests.Include(g => g.Account).SingleOrDefaultAsync(g => g.Email == email);
            }
            else
            {
                return await _dbContext.Guests.SingleOrDefaultAsync(g => g.Email == email);
            }
        }

        public async Task AddGuest(Guest guest)
        {
            _dbContext.Guests.Add(guest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateGuest(Guest guest)
        {
            _dbContext.Guests.Update(guest);
            await _dbContext.SaveChangesAsync();
        }
    }
}
