using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Response;
using server.Interfaces.Services;
using server.Utilities;

namespace server.Controllers
{
    [ApiController]
    [Route("/statistic")]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("")]
        public async Task<IActionResult> GetSummaryStatistic([FromQuery] string type)
        {
            var result = await _statisticService.GetSummaryStatistic(type);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Data = result.Data });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularStatistic([FromQuery] string type)
        {
            var result = await _statisticService.GetPopularStatistic(type);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Data = result.Data });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("revenues")]
        public async Task<IActionResult> GetRevenuesChart([FromQuery] string type)
        {
            var result = await _statisticService.GetRevenuesChart(type, "vi");
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Data = result.Data });
        }
    }
}
