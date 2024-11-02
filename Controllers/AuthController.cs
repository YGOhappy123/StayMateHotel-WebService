using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Auth;
using server.Dtos.Response;
using server.Interfaces.Services;
using server.Mappers;
using server.Models;
using server.Utilities;

namespace server.Controllers
{
    [ApiController]
    [Route("/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto signInDto)
        {
            var result = await _authService.SignIn(signInDto);

            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            if (result.Data is Admin admin)
            {
                return StatusCode(
                    result.Status,
                    new SuccessResponseDto
                    {
                        Message = result.Message,
                        Data = new { User = admin.ToAdminDto() },
                    }
                );
            }
            else if (result.Data is Guest guest)
            {
                return StatusCode(
                    result.Status,
                    new SuccessResponseDto
                    {
                        Message = result.Message,
                        Data = new { User = guest.ToGuestDto() },
                    }
                );
            }
            else
            {
                return StatusCode(
                    ResStatusCode.BAD_REQUEST,
                    new ErrorResponseDto { Message = result.Message }
                );
            }
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
        {
            var result = await _authService.SignUpGuestAccount(signUpDto);

            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(
                result.Status,
                new SuccessResponseDto
                {
                    Message = result.Message,
                    Data = new { User = result.Data!.ToGuestDto() },
                }
            );
        }
    }
}
