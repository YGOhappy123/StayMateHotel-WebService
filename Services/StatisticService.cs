using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Response;
using server.Dtos.Room;
using server.Extensions;
using server.Interfaces.Repositories;
using server.Interfaces.Services;
using server.Models;
using server.Utilities;

namespace server.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IGuestRepository _guestRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly IReservationRepository _reservationRepo;
        private readonly IPaymentRepository _paymentRepo;

        public StatisticService(
            IGuestRepository guestRepo,
            IRoomRepository roomRepo,
            IReservationRepository reservationRepo,
            IPaymentRepository paymentRepo
        )
        {
            _guestRepo = guestRepo;
            _roomRepo = roomRepo;
            _reservationRepo = reservationRepo;
            _paymentRepo = paymentRepo;
        }

        private class ChartParams
        {
            public int Columns { get; set; } = 1;
            public string TimeUnit { get; set; } = string.Empty;
            public string Format { get; set; } = string.Empty;

            public ChartParams(int columns, string timeUnit, string format)
            {
                Columns = columns;
                TimeUnit = timeUnit;
                Format = format;
            }
        }

        private class ChartItem
        {
            public DateTime Date { get; set; } = DateTime.Now;
            public string Name { get; set; } = string.Empty;
            public int BookingCount { get; set; } = 0;
            public decimal TotalRevenues { get; set; } = 0;

            public ChartItem(DateTime date, string name, int bookingCount, decimal totalRevenues)
            {
                Date = date;
                Name = name;
                BookingCount = bookingCount;
                TotalRevenues = totalRevenues;
            }
        }

        public async Task<ServiceResponse<object>> GetSummaryStatistic(string type)
        {
            var currentTime = TimestampHandler.GetNow();
            var previousTime = TimestampHandler.GetPreviousTimeByType(currentTime, type);
            var startOfCurrentTime = TimestampHandler.GetStartOfTimeByType(currentTime, type);
            var startOfPreviousTime = TimestampHandler.GetStartOfTimeByType(previousTime, type);

            var currGuestsCount = await _guestRepo.CountGuestsCreatedInTimeRange(startOfCurrentTime, currentTime);
            var prevGuestsCount = await _guestRepo.CountGuestsCreatedInTimeRange(startOfPreviousTime, startOfCurrentTime);

            var currBookingsCount = await _reservationRepo.CountBookingsMadeInTimeRange(startOfCurrentTime, currentTime);
            var prevBookingsCount = await _reservationRepo.CountBookingsMadeInTimeRange(startOfPreviousTime, startOfCurrentTime);

            var currRevenues = await _paymentRepo.SumPaymentValuesInTimeRange(startOfCurrentTime, currentTime);
            var prevRevenues = await _paymentRepo.SumPaymentValuesInTimeRange(startOfPreviousTime, startOfCurrentTime);

            return new ServiceResponse<object>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = new
                {
                    Guests = new { CurrentCount = currGuestsCount, PreviousCount = prevGuestsCount },
                    Bookings = new { CurrentCount = currBookingsCount, PreviousCount = prevBookingsCount },
                    Revenues = new { CurrentCount = currRevenues, PreviousCount = prevRevenues },
                },
            };
        }

        public async Task<ServiceResponse<object>> GetPopularStatistic(string type)
        {
            var currentTime = TimestampHandler.GetNow();
            var startOfCurrentTime = TimestampHandler.GetStartOfTimeByType(currentTime, type);

            var mostBookedRooms = await _roomRepo.GetMostBookedRoomsInTimeRange(startOfCurrentTime, currentTime, 5);
            var highestBookingCountGuests = await _guestRepo.GetGuestsWithHighestBookingCountInTimeRange(
                startOfCurrentTime,
                currentTime,
                5
            );
            var highestTotalPaymentGuests = await _guestRepo.GetGuestsWithHighestPaymentAmountInTimeRange(
                startOfCurrentTime,
                currentTime,
                5
            );

            return new ServiceResponse<object>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = new
                {
                    MostBookedRooms = mostBookedRooms.Select(room => new
                    {
                        room.Id,
                        room.RoomNumber,
                        room.CreatedAt,
                        room.BookingCount,
                        Status = room.Status.ToString(),
                        Images = room.Images.Select(img => img.ImageUrl).ToList(),
                        Floor = room?.Floor == null ? null : new RoomFloorInfo { Id = room.Floor.Id, FloorNumber = room.Floor.FloorNumber },
                        RoomClass = room?.RoomClass == null
                            ? null
                            : new RoomRoomClassInfo
                            {
                                Id = room.RoomClass.Id,
                                ClassName = room.RoomClass.ClassName,
                                BasePrice = room.RoomClass.BasePrice,
                                Capacity = room.RoomClass.Capacity,
                            },
                    }),

                    HighestBookingCountGuests = highestBookingCountGuests.Select(guest => new
                    {
                        guest.Id,
                        guest.FirstName,
                        guest.LastName,
                        guest.Email,
                        guest.Avatar,
                        guest.CreatedAt,
                        guest.PhoneNumber,
                        guest.Address,
                        guest.BookingCount,
                        IsActive = guest.Account != null && guest.Account.IsActive,
                    }),

                    HighestTotalPaymentGuests = highestTotalPaymentGuests.Select(guest => new
                    {
                        guest.Id,
                        guest.FirstName,
                        guest.LastName,
                        guest.Email,
                        guest.Avatar,
                        guest.CreatedAt,
                        guest.PhoneNumber,
                        guest.Address,
                        guest.TotalPayment,
                        IsActive = guest.Account != null && guest.Account.IsActive,
                    }),
                },
            };
        }

        public async Task<ServiceResponse<object>> GetRevenuesChart(string type, string locale)
        {
            var currentTime = TimestampHandler.GetNow();
            var startOfCurrentTime = TimestampHandler.GetStartOfTimeByType(currentTime, type);

            var bookings = await _reservationRepo.GetBookingsMadeInTimeRange(startOfCurrentTime, currentTime);
            var payments = await _paymentRepo.GetPaymentsMadeInTimeRange(startOfCurrentTime, currentTime);
            var revenuesChart = CreateRevenuesChart(
                bookings,
                payments,
                startOfCurrentTime,
                PrepareCreateChartParams(type, startOfCurrentTime),
                locale
            );

            return new ServiceResponse<object>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = revenuesChart.Select(ci => new
                {
                    Name = ci.Name.CapitalizeEachWords(),
                    ci.BookingCount,
                    ci.TotalRevenues,
                }),
            };
        }

        private ChartParams PrepareCreateChartParams(string type, DateTime startDate)
        {
            return type.ToLower() switch
            {
                "daily" => new ChartParams(24, "hour", "HH:mm"),
                "weekly" => new ChartParams(7, "day", "dddd dd-MM"),
                "monthly" => new ChartParams(startDate.GetDaysInMonth(), "day", "dd"),
                "yearly" => new ChartParams(12, "month", "MMMM"),
                _ => throw new ArgumentException("Invalid type"),
            };
        }

        private List<ChartItem> CreateRevenuesChart(
            List<Booking> bookings,
            List<Payment> payments,
            DateTime startDate,
            ChartParams chartParams,
            string locale
        )
        {
            CultureInfo cultureInfo = new CultureInfo(locale == "vi" ? "vi-VN" : "en-US");
            List<ChartItem> chartItems = [];

            for (int i = 0; i < chartParams.Columns; i++)
            {
                chartItems.Add(
                    new ChartItem(
                        startDate.AddByUnitAndAmount(i, chartParams.TimeUnit),
                        startDate.AddByUnitAndAmount(i, chartParams.TimeUnit).ToString(chartParams.Format, cultureInfo),
                        0,
                        0
                    )
                );
            }

            foreach (var booking in bookings)
            {
                var index = chartItems.FindIndex(item => TimestampHandler.IsSame(item.Date, booking.CreatedAt, chartParams.TimeUnit));
                chartItems[index].BookingCount += 1;
            }

            foreach (var payment in payments)
            {
                var index = chartItems.FindIndex(item => TimestampHandler.IsSame(item.Date, payment.PaymentTime, chartParams.TimeUnit));
                chartItems[index].TotalRevenues += payment.Amount;
            }

            return chartItems;
        }
    }
}
