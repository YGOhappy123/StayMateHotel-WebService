using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Response;
using server.Dtos.RoomClass;
using server.Extensions.Mappers;
using server.Interfaces.Services;
using server.Models;
using server.Queries;
using server.Utilities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace server.Controllers
{
    [ApiController]
    [Route("/roomClasses")]
    public class RoomClassController : ControllerBase
    {
        private readonly IRoomClassService _roomClassService;

        public RoomClassController(IRoomClassService roomClassService)
        {
            _roomClassService = roomClassService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoomClasses([FromQuery] BaseQueryObject queryObject)
        {
            var result = await _roomClassService.GetAllRoomClasses(queryObject);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(
                result.Status,
                new SuccessResponseDto
                {
                    Data = result.Data!.Select(rm => rm.ToRoomClassDto()),
                    Total = result.Total,
                    Took = result.Took,
                }
            );
        }

        [HttpGet("{roomClassId:int}")]
        public async Task<IActionResult> GetRoomClassById([FromRoute] int roomClassId)
        {
            var result = await _roomClassService.GetRoomClassById(roomClassId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Data = result.Data!.ToRoomClassDto() });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateNewRoomClass([FromBody] CreateUpdateRoomClassDto createRoomClassDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var authUserId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var result = await _roomClassService.CreateNewRoomClass(createRoomClassDto, int.Parse(authUserId!));
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{roomClassId:int}")]
        public async Task<IActionResult> UpdateRoomClass([FromRoute] int roomClassId, [FromBody] CreateUpdateRoomClassDto updateRoomClassDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var result = await _roomClassService.UpdateRoomClass(roomClassId, updateRoomClassDto);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{roomClassId:int}")]
        public async Task<IActionResult> DeleteRoomClass([FromRoute] int roomClassId)
        {
            var result = await _roomClassService.DeleteRoomClass(roomClassId);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }
    }
}
