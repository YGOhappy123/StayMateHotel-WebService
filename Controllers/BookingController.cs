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
        public async Task<IActionResult> GetAllRooms([FromQuery] BaseQueryObject queryObject)
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

            return StatusCode(result.Status, new SuccessResponseDto { Data = result.Data });
        }
    }
}
