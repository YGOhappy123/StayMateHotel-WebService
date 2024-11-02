using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Account;
using server.Models;

namespace server.Mappers
{
    public static class GuestMapper
    {
        public static GuestDto ToGuestDto(this Guest guest)
        {
            return new GuestDto
            {
                Id = guest.Id,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                Email = guest.Email,
                Avatar = guest.Avatar,
                CreatedAt = guest.CreatedAt,
                PhoneNumber = guest.PhoneNumber,
                Address = guest.Address,
            };
        }
    }
}