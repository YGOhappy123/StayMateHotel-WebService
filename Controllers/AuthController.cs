using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Auth;
using server.Dtos.Response;
using server.Interfaces.Services;

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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] SignUpDto signUpDto)
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
                    Data = new { User = result.Data },
                }
            );
        }
    }
}
