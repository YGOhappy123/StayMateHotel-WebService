using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.File;
using server.Dtos.Response;
using server.Interfaces.Services;
using server.Utilities;

namespace server.Controllers
{
    [ApiController]
    [Route("/file")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadSingleImage([FromForm] UploadImageDto uploadImageDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var result = await _fileService.UploadImageToCloudinary(uploadImageDto.File, "rooms");
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message, Data = new { result.ImageUrl } });
        }

        [HttpPost("delete-image")]
        public async Task<IActionResult> DeleteSingleImage([FromBody] DeleteImageDto deleteImageDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(
                    ResStatusCode.UNPROCESSABLE_ENTITY,
                    new ErrorResponseDto { Message = ErrorMessage.DATA_VALIDATION_FAILED }
                );
            }

            var result = await _fileService.DeleteImageFromCloudinary(deleteImageDto.ImageUrl);
            if (!result.Success)
            {
                return StatusCode(result.Status, new ErrorResponseDto { Message = result.Message });
            }

            return StatusCode(result.Status, new SuccessResponseDto { Message = result.Message });
        }
    }
}
