using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Reservation;
using server.Dtos.Response;
using server.Extensions.Mappers;
using server.Interfaces.Services;
using server.Queries;
using server.Utilities;

namespace server.Controllers
{
    [ApiController]
    [Route("/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public BookingController(IReservationService ReservationService)
        {
            _reservationService = ReservationService;
        }

        [HttpGet("available-rooms")]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] BaseQueryObject queryObject)
        {
            var result = await _reservationService.FindAvailableRooms(queryObject);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(
                result.Status,
                new SuccessResponseDto { Data = result.Data!.Select(rmList => rmList.Select(rm => rm.ToRoomDto())) }
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllBookings([FromQuery] BaseQueryObject queryObject)
        {
            var result = await _reservationService.GetAllBookings(queryObject);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(
                result.Status,
                new SuccessResponseDto
                {
                    Data = result.Data!.Select(bk => bk.ToBookingDto()),
                    Total = result.Total,
                    Took = result.Took,
                }
            );
        }

        [Authorize(Roles = "Guest")]
        [HttpGet("my-bookings")]
        public async Task<IActionResult> GetCustomerBookings()
        {
            var authUserId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            var result = await _reservationService.GetMyBookings(int.Parse(authUserId!));
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Data = result.Data!.Select(bk => bk.ToBookingDto()) });
        }

        [Authorize(Roles = "Guest")]
        [HttpPost("make-booking")]
        public async Task<IActionResult> MakeNewBooking(MakeBookingDto makeBookingDto)
        {
            var authUserId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            var result = await _reservationService.MakeNewBooking(makeBookingDto, int.Parse(authUserId!));
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Data = result.Data, Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("accept-booking/{bookingId:int}")]
        public async Task<IActionResult> AcceptBooking([FromRoute] int bookingId)
        {
            var result = await _reservationService.AcceptBooking(bookingId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize]
        [HttpPost("cancel-booking/{bookingId:int}")]
        public async Task<IActionResult> CancelBooking([FromRoute] int bookingId)
        {
            var authUserId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var authUserRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _reservationService.CancelBooking(bookingId, int.Parse(authUserId!), authUserRole!);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("check-in/{bookingId:int}")]
        public async Task<IActionResult> CheckInBooking([FromRoute] int bookingId)
        {
            var result = await _reservationService.CheckInBooking(bookingId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("check-out/{bookingId:int}")]
        public async Task<IActionResult> CheckOutBooking([FromRoute] int bookingId)
        {
            var result = await _reservationService.CheckOutBooking(bookingId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("deposit/{bookingId:int}")]
        public async Task<IActionResult> DepositBooking([FromRoute] int bookingId, [FromBody] DepositPaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var result = await _reservationService.DepositBooking(bookingId, paymentDto);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("make-payment/{bookingId:int}")]
        public async Task<IActionResult> MakePaymentBooking([FromRoute] int bookingId, [FromBody] MakePaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var result = await _reservationService.MakePaymentBooking(bookingId, paymentDto);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("count-by-status")]
        public async Task<IActionResult> CountBookingsByStatus([FromQuery] TimeRangeQueryObject queryObject)
        {
            var result = await _reservationService.CountBookingsByStatus(queryObject);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Data = result.Data });
        }
    }
}
