using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Dtos.Statistic
{
    public class GuestWithTotalPayment : Guest
    {
        public decimal TotalPayment { get; set; }

        public GuestWithTotalPayment(Guest guest, decimal totalPayment)
        {
            this.Id = guest.Id;
            this.FirstName = guest.FirstName;
            this.LastName = guest.LastName;
            this.Email = guest.Email;
            this.Avatar = guest.Avatar;
            this.CreatedAt = guest.CreatedAt;
            this.AccountId = guest.AccountId;
            this.Account = guest.Account;
            this.PhoneNumber = guest.PhoneNumber;
            this.Address = guest.Address;
            this.Bookings = guest.Bookings;
            this.TotalPayment = totalPayment;
        }
    }
}
