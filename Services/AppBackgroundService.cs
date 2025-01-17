using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Enums;
using server.Repositories;

namespace server.Services
{
    public class AppBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AppBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await HandleUnhandledBookings();

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRun = DateTime.Today.AddDays(1).AddMinutes(1);
                var delay = nextRun - now;

                await Task.Delay(delay, stoppingToken);
                await HandleUnhandledBookings();
            }
        }

        private async Task HandleUnhandledBookings()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
            var today = DateTime.Today;

            await RejectPendingBookingsAfterCheckInDate(dbContext, today);
            await RejectConfirmedBookingsAfterCheckOutDate(dbContext, today);
            await CheckOutCheckedInBookingsAfterCheckOutDate(dbContext, today);
        }

        private async Task RejectPendingBookingsAfterCheckInDate(ApplicationDBContext dbContext, DateTime today)
        {
            var bookings = await dbContext.Bookings.Where(bk => bk.Status == BookingStatus.Pending && bk.CheckInTime < today).ToListAsync();
            foreach (var booking in bookings)
            {
                booking.Status = BookingStatus.Cancelled;
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task RejectConfirmedBookingsAfterCheckOutDate(ApplicationDBContext dbContext, DateTime today)
        {
            var bookings = await dbContext
                .Bookings.Where(bk => bk.Status == BookingStatus.Confirmed && bk.CheckOutTime < today)
                .ToListAsync();
            foreach (var booking in bookings)
            {
                booking.Status = BookingStatus.Cancelled;
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task CheckOutCheckedInBookingsAfterCheckOutDate(ApplicationDBContext dbContext, DateTime today)
        {
            var bookings = await dbContext
                .Bookings.Where(bk => bk.Status == BookingStatus.CheckedIn && bk.CheckOutTime < today)
                .ToListAsync();
            foreach (var booking in bookings)
            {
                booking.Status = BookingStatus.CheckedOut;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
