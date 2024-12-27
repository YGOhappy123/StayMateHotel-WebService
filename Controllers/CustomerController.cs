using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Response;
using server.Dtos.User;
using server.Extensions.Mappers;
using server.Interfaces.Services;
using server.Queries;
using server.Utilities;

namespace server.Controllers
{
    [ApiController]
    [Route("/guests")]
    public class GuestController : ControllerBase
    {
        private readonly IUserService _userService;

        public GuestController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("")]
        public async Task<IActionResult> GetAllGuests([FromQuery] BaseQueryObject queryObject)
        {
            var result = await _userService.GetAllGuests(queryObject);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(
                result.Status,
                new SuccessResponseDto
                {
                    Data = result.Data!.Select(g => g.ToGuestDto()),
                    Total = result.Total,
                    Took = result.Took,
                }
            );
        }

        [Authorize(Roles = "Guest")]
        [HttpPatch("profile")]
        public async Task<IActionResult> UpdateGuestProfile([FromBody] UpdateGuestDto updateGuestDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var authUserId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            var result = await _userService.UpdateGuestProfile(updateGuestDto, int.Parse(authUserId!));
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }
    }
}
