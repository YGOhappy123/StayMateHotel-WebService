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
    [Route("/admins")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("")]
        public async Task<IActionResult> GetAllAdmins([FromQuery] BaseQueryObject queryObject)
        {
            var result = await _userService.GetAllAdmins(queryObject);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(
                result.Status,
                new SuccessResponseDto
                {
                    Data = result.Data!.Select(ad => ad.ToAdminDto()),
                    Total = result.Total,
                    Took = result.Took,
                }
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("")]
        public async Task<IActionResult> CreateNewAdmin([FromBody] CreateAdminDto createAdminDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var authUserId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            var result = await _userService.CreateNewAdmin(createAdminDto, int.Parse(authUserId!));
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("profile")]
        public async Task<IActionResult> UpdateAdminProfile([FromBody] UpdateAdminDto updateAdminDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var authUserId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            var result = await _userService.UpdateAdminProfile(updateAdminDto, int.Parse(authUserId!));
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("toggle-active/{adminId:int}")]
        public async Task<IActionResult> ToggleAdminActiveStatus([FromRoute] int adminId)
        {
            var authUserId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            var result = await _userService.ToggleAdminActiveStatus(adminId, int.Parse(authUserId!));
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }
    }
}
