using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Dtos.Statistic
{
    public class BookingWithTotalPayment : Booking
    {
        public decimal TotalPayment { get; set; }

        public BookingWithTotalPayment(Booking booking, decimal totalPayment)
        {
            this.Id = booking.Id;
            this.CheckInTime = booking.CheckInTime;
            this.CheckOutTime = booking.CheckOutTime;
            this.Email = booking.Email;
            this.PhoneNumber = booking.PhoneNumber;
            this.Status = booking.Status;
            this.TotalAmount = booking.TotalAmount;
            this.CreatedAt = booking.CreatedAt;
            this.GuestId = booking.GuestId;
            this.Guest = booking.Guest;
            this.Payments = booking.Payments;
            this.BookingRooms = booking.BookingRooms;
            this.BookingServices = booking.BookingServices;
            this.TotalPayment = totalPayment;
        }
    }
}
