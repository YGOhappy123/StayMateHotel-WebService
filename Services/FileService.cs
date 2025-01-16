using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using server.Dtos.Response;
using server.Interfaces.Services;
using server.Utilities;

namespace server.Services
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly Cloudinary _cloudinary;

        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;

            var account = new Account(
                _configuration["Cloudinary:CloudName"],
                _configuration["Cloudinary:ApiKey"],
                _configuration["Cloudinary:ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
        }

        private string GetPublicIdFromUrl(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var segments = uri.AbsolutePath.Split('/');
            var publicIdWithExtension = segments.Last();
            var publicId = publicIdWithExtension.Substring(0, publicIdWithExtension.LastIndexOf('.'));

            return string.Join("/", segments.Skip(segments.Length - 2).Take(1)) + "/" + publicId;
        }

        public async Task<ServiceResponse> UploadImageToCloudinary(IFormFile imageFile, string? folderName)
        {
            try
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream()),
                    Folder = string.IsNullOrWhiteSpace(folderName) ? "stay-mate-hotel" : folderName,
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                bool isSuccess = uploadResult.StatusCode == System.Net.HttpStatusCode.OK;

                return new ServiceResponse
                {
                    Status = isSuccess ? ResStatusCode.OK : ResStatusCode.INTERNAL_SERVER_ERROR,
                    Success = isSuccess,
                    Message = isSuccess ? SuccessMessage.UPLOAD_IMAGE_SUCCESSFULLY : ErrorMessage.UPLOAD_IMAGE_FAILED,
                    ImageUrl = isSuccess ? uploadResult.SecureUrl.ToString() : "",
                };
            }
            catch (Exception)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.INTERNAL_SERVER_ERROR,
                    Success = false,
                    Message = ErrorMessage.UPLOAD_IMAGE_FAILED,
                };
            }
        }

        public async Task<ServiceResponse> UploadBase64ImageToCloudinary(string base64Image, string? folderName)
        {
            try
            {
                if (!base64Image.StartsWith("data:image/"))
                {
                    base64Image = "data:image/jpeg;base64," + base64Image;
                }

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(base64Image),
                    Folder = string.IsNullOrWhiteSpace(folderName) ? "stay-mate-hotel" : folderName,
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                bool isSuccess = uploadResult.StatusCode == System.Net.HttpStatusCode.OK;

                return new ServiceResponse
                {
                    Status = isSuccess ? ResStatusCode.OK : ResStatusCode.INTERNAL_SERVER_ERROR,
                    Success = isSuccess,
                    Message = isSuccess ? SuccessMessage.UPLOAD_IMAGE_SUCCESSFULLY : ErrorMessage.UPLOAD_IMAGE_FAILED,
                    ImageUrl = isSuccess ? uploadResult.SecureUrl.ToString() : "",
                };
            }
            catch (Exception)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.INTERNAL_SERVER_ERROR,
                    Success = false,
                    Message = ErrorMessage.UPLOAD_IMAGE_FAILED,
                };
            }
        }

        public async Task<ServiceResponse> DeleteImageFromCloudinary(string imageUrl)
        {
            try
            {
                var publicId = GetPublicIdFromUrl(imageUrl);
                var deletionParams = new DeletionParams(publicId);

                var deletionResult = await _cloudinary.DestroyAsync(deletionParams);
                bool isSuccess = deletionResult.StatusCode == System.Net.HttpStatusCode.OK;

                return new ServiceResponse
                {
                    Status = isSuccess ? ResStatusCode.OK : ResStatusCode.INTERNAL_SERVER_ERROR,
                    Success = isSuccess,
                    Message = isSuccess ? SuccessMessage.DELETE_IMAGE_SUCCESSFULLY : ErrorMessage.DELETE_IMAGE_FAILED,
                };
            }
            catch (Exception)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.INTERNAL_SERVER_ERROR,
                    Success = false,
                    Message = ErrorMessage.DELETE_IMAGE_FAILED,
                };
            }
        }
    }
}
