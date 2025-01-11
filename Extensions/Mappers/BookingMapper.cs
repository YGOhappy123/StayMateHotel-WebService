using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Reservation;
using server.Models;

namespace server.Extensions.Mappers
{
    public static class BookingMapper
    {
        public static BookingDto ToBookingDto(this Booking booking)
        {
            return new BookingDto
            {
                Id = booking.Id,
                CheckInTime = booking.CheckInTime,
                CheckOutTime = booking.CheckOutTime,
                Email = booking.Email,
                PhoneNumber = booking.PhoneNumber,
                Status = booking.Status.ToString(),
                TotalAmount = booking.TotalAmount,
                CreatedAt = booking.CreatedAt,
                Guest =
                    booking?.Guest == null
                        ? null
                        : new GuestInfo
                        {
                            Id = booking.Guest.Id,
                            FirstName = booking.Guest.FirstName,
                            LastName = booking.Guest.LastName,
                        },
                Payments =
                    booking?.Payments == null
                        ? null
                        : booking
                            .Payments.Select(pm => new PaymentInfo
                            {
                                Id = pm.Id,
                                Amount = pm.Amount,
                                PaymentTime = pm.PaymentTime,
                                Method = pm.Method.ToString(),
                            })
                            .ToList(),
                BookingRooms =
                    booking?.BookingRooms == null
                        ? null
                        : booking
                            .BookingRooms.Select(bkr => new BookingRoomInfo
                            {
                                NumberOfGuests = bkr.NumberOfGuests,
                                UnitPrice = bkr.UnitPrice,
                                RoomNumber = bkr?.Room?.RoomNumber,
                                Floor = bkr?.Room?.Floor?.FloorNumber,
                                RoomClass = bkr?.Room?.RoomClass?.ClassName,
                            })
                            .ToList(),
                BookingServices =
                    booking?.BookingServices == null
                        ? null
                        : booking
                            .BookingServices.Select(bks => new BookingServiceInfo
                            {
                                Id = bks.Id,
                                Quantity = bks.Quantity ?? 1,
                                UnitPrice = bks.UnitPrice,
                                CreatedAt = bks.CreatedAt,
                                Status = bks.Status.ToString(),
                                Name = bks?.Service?.Name,
                            })
                            .ToList(),
            };
        }

        public static PaymentDto ToPaymentDto(this Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                Amount = payment.Amount,
                PaymentTime = payment.PaymentTime,
                Method = payment.Method.ToString(),
                BookingId = payment.BookingId ?? 0,
            };
        }

        public static BookingServiceDto ToBookingServiceDto(this BookingService bookingService)
        {
            return new BookingServiceDto
            {
                Id = bookingService.Id,
                Quantity = bookingService.Quantity,
                UnitPrice = bookingService.UnitPrice,
                Status = bookingService.Status.ToString(),
                CreatedAt = bookingService.CreatedAt,
                Booking =
                    bookingService.Booking == null
                        ? null
                        : new BookingInfo
                        {
                            Id = bookingService.Booking.Id,
                            Email = bookingService.Booking.Email,
                            PhoneNumber = bookingService.Booking.PhoneNumber,
                            Guest =
                                bookingService.Booking?.Guest == null
                                    ? null
                                    : new GuestInfo
                                    {
                                        Id = bookingService.Booking.Guest.Id,
                                        FirstName = bookingService.Booking.Guest.FirstName,
                                        LastName = bookingService.Booking.Guest.LastName,
                                    },
                        },
                Service =
                    bookingService.Service == null
                        ? null
                        : new ServiceInfo { Id = (int)bookingService.ServiceId!, Name = bookingService.Service.Name },
            };
        }
    }
}
