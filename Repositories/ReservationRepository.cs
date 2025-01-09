using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Enums;
using server.Interfaces.Repositories;
using server.Models;
using server.Queries;
using server.Utilities;

namespace server.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IRoomRepository _roomRepo;

        public ReservationRepository(ApplicationDBContext context, IRoomRepository roomRepo)
        {
            _dbContext = context;
            _roomRepo = roomRepo;
        }

        private IQueryable<Booking> ApplyFilters(IQueryable<Booking> query, Dictionary<string, object> filters)
        {
            foreach (var filter in filters)
            {
                string value = filter.Value.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(value))
                {
                    switch (filter.Key)
                    {
                        case "startBookingTime":
                            query = query.Where(bk => bk.CreatedAt >= DateTime.Parse(value));
                            break;
                        case "endBookingTime":
                            query = query.Where(bk => bk.CreatedAt <= TimestampHandler.GetEndOfTimeByType(DateTime.Parse(value), "daily"));
                            break;
                        case "startCheckInTime":
                            query = query.Where(bk => bk.CheckInTime >= DateTime.Parse(value));
                            break;
                        case "endCheckInTime":
                            query = query.Where(bk =>
                                bk.CheckInTime <= TimestampHandler.GetEndOfTimeByType(DateTime.Parse(value), "daily")
                            );
                            break;
                        case "startCheckOutTime":
                            query = query.Where(bk => bk.CheckOutTime >= DateTime.Parse(value));
                            break;
                        case "endCheckOutTime":
                            query = query.Where(bk =>
                                bk.CheckOutTime <= TimestampHandler.GetEndOfTimeByType(DateTime.Parse(value), "daily")
                            );
                            break;
                        case "roomNumber":
                            query = query.Where(bk => bk.BookingRooms.Any(bkr => bkr.RoomId == int.Parse(value)));
                            break;
                        case "minTotalAmount":
                            query = query.Where(bk => bk.TotalAmount >= Convert.ToDecimal(value));
                            break;
                        case "maxTotalAmount":
                            query = query.Where(bk => bk.TotalAmount <= Convert.ToDecimal(value));
                            break;
                        case "email":
                            query = query.Where(bk => bk.Email.Contains(value));
                            break;
                        case "phoneNumber":
                            query = query.Where(bk => bk.PhoneNumber.Contains(value));
                            break;
                        case "guestName":
                            query = query.Where(bk =>
                                bk.Guest != null && (bk.Guest.FirstName.Contains(value) || bk.Guest.LastName.Contains(value))
                            );
                            break;
                        case "status":
                            query = query.Where(bk => bk.Status == Enum.Parse<BookingStatus>(value));
                            break;
                        default:
                            query = query.Where(bk => EF.Property<string>(bk, filter.Key.CapitalizeWord()) == value);
                            break;
                    }
                }
            }

            return query;
        }

        private IQueryable<Booking> ApplySorting(IQueryable<Booking> query, Dictionary<string, string> sort)
        {
            foreach (var order in sort)
            {
                query =
                    order.Value == "ASC"
                        ? query.OrderBy(mt => EF.Property<object>(mt, order.Key.CapitalizeWord()))
                        : query.OrderByDescending(mt => EF.Property<object>(mt, order.Key.CapitalizeWord()));
            }

            return query;
        }

        public async Task<List<Room>> FindAvailableRooms(DateTime checkInDate, DateTime checkOutDate, int numberOfGuests)
        {
            // Find all Not-Available bookings in time range
            // Extract the room ids from them
            var overlappingBookingRoomIds = await _dbContext
                .Bookings.Where(bk =>
                    bk.Status != BookingStatus.Pending
                    && bk.Status != BookingStatus.Cancelled
                    && bk.CheckOutTime >= checkInDate
                    && bk.CheckInTime <= checkOutDate
                )
                .SelectMany(bk => bk.BookingRooms.Select(br => br.RoomId))
                .Distinct()
                .ToListAsync();

            var roomIds = await _dbContext
                .Rooms.Where(rm =>
                    !overlappingBookingRoomIds.Contains(rm.Id)
                    && rm.RoomClass!.Capacity >= numberOfGuests
                    && rm.Status != RoomStatus.OutOfService
                )
                .OrderBy(rm => rm.RoomClass!.BasePrice)
                .Select(rm => rm.Id)
                .ToListAsync();

            List<Room> result = [];
            foreach (var roomId in roomIds)
            {
                var room = await _roomRepo.GetRoomById(roomId);
                if (room != null)
                {
                    result.Add(room);
                }
            }

            return result;
        }

        public async Task<(List<Booking>, int)> GetAllBookings(BaseQueryObject queryObject)
        {
            var query = _dbContext
                .Bookings.Include(bk => bk.Guest)
                .Include(bk => bk.BookingRooms)
                .ThenInclude(bkr => bkr.Room)
                .ThenInclude(rm => rm.Floor)
                .Include(bk => bk.BookingRooms)
                .ThenInclude(bkr => bkr.Room)
                .ThenInclude(rm => rm.RoomClass)
                .Include(bk => bk.BookingServices)
                .ThenInclude(bks => bks.Service)
                .Include(bk => bk.Payments)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject.Filter))
            {
                var parsedFilter = JsonSerializer.Deserialize<Dictionary<string, object>>(queryObject.Filter);
                query = ApplyFilters(query, parsedFilter!);
            }

            if (!string.IsNullOrWhiteSpace(queryObject.Sort))
            {
                var parsedSort = JsonSerializer.Deserialize<Dictionary<string, string>>(queryObject.Sort);
                query = ApplySorting(query, parsedSort!);
            }

            var total = await query.CountAsync();

            if (queryObject.Skip.HasValue)
                query = query.Skip(queryObject.Skip.Value);

            if (queryObject.Limit.HasValue)
                query = query.Take(queryObject.Limit.Value);

            var bookings = await query.ToListAsync();

            return (bookings, total);
        }

        public async Task<List<Booking>> GetMyBookings(int guestId)
        {
            return await _dbContext
                .Bookings.Include(bk => bk.Guest)
                .Include(bk => bk.BookingRooms)
                .ThenInclude(bkr => bkr.Room)
                .ThenInclude(rm => rm.Floor)
                .Include(bk => bk.BookingRooms)
                .ThenInclude(bkr => bkr.Room)
                .ThenInclude(rm => rm.RoomClass)
                .Include(bk => bk.BookingServices)
                .ThenInclude(bks => bks.Service)
                .Include(bk => bk.Payments)
                .Where(bk => bk.GuestId == guestId)
                .ToListAsync();
        }

        public async Task CreateNewBooking(Booking booking)
        {
            _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Booking?> GetBookingById(int bookingId)
        {
            return await _dbContext
                .Bookings.Include(bk => bk.BookingRooms)
                .ThenInclude(bkr => bkr.Room)
                .Include(bk => bk.BookingServices)
                .ThenInclude(bks => bks.Service)
                .Include(bk => bk.Payments)
                .Where(bk => bk.Id == bookingId)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateBooking(Booking booking)
        {
            _dbContext.Bookings.Update(booking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CancelReservationsWithDuplicateRooms(DateTime checkInDate, DateTime checkOutDate, int bookingId, int roomId)
        {
            // Find all Pending bookings in time range
            var overlappingPendingBookingIds = await _dbContext
                .Bookings.Where(bk =>
                    bk.Status == BookingStatus.Pending
                    && bk.CheckOutTime >= checkInDate
                    && bk.CheckInTime <= checkOutDate
                    && bk.Id != bookingId
                )
                .Select(bk => bk.Id)
                .ToListAsync();

            foreach (var id in overlappingPendingBookingIds)
            {
                var isRoomBooked = await _dbContext.BookingRooms.AnyAsync(bkr => bkr.BookingId == id && bkr.RoomId == roomId);
                if (isRoomBooked)
                {
                    var booking = await _dbContext.Bookings.Where(bk => bk.Id == id).FirstOrDefaultAsync();
                    if (booking != null && booking.Status == BookingStatus.Pending)
                    {
                        booking.Status = BookingStatus.Cancelled;
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task<int> CountBookingsByStatus(BookingStatus status, TimeRangeQueryObject queryObject)
        {
            var query = _dbContext.Bookings.Where(bk => bk.Status == status).AsQueryable();

            if (queryObject.StartTime != null)
            {
                query = query.Where(bk => bk.CreatedAt >= queryObject.StartTime.Value);
            }
            if (queryObject.EndTime != null)
            {
                query = query.Where(bk => bk.CreatedAt <= TimestampHandler.GetEndOfTimeByType(queryObject.EndTime.Value, "daily"));
            }

            return await query.CountAsync();
        }
    }
}
