using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
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

            object userDto =
                result.Data is Admin
                    ? ((Admin)result.Data!).ToAdminDto()
                    : ((Guest)result.Data!).ToGuestDto();

            return StatusCode(
                result.Status,
                new SuccessResponseDto
                {
                    Message = result.Message,
                    Data = new
                    {
                        User = userDto,
                        result.AccessToken,
                        result.RefreshToken,
                    },
                }
            );
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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = await _authService.RefreshToken(refreshTokenDto);

            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(
                result.Status,
                new SuccessResponseDto
                {
                    Message = result.Message,
                    Data = new { result.AccessToken },
                }
            );
        }

        [Authorize]
        [HttpGet("test-login")]
        public IActionResult Test()
        {
            var authUserId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var authUserRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(
                new
                {
                    UserId = authUserId,
                    Role = authUserRole,
                    Content = "login content only",
                }
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("test-admin")]
        public IActionResult TestAdmin()
        {
            return Ok("admin content only");
        }
    }
}
