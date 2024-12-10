using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Response;
using server.Dtos.Feature;
using server.Enums;
using server.Interfaces.Repositories;
using server.Interfaces.Services;
using server.Models;
using server.Queries;
using server.Utilities;

namespace server.Services
{
    public class FeatureService : IFeatureService
    {
        private readonly IFeatureRepository _featureRepo;
        private readonly IRoomClassFeatureRepository _roomClassFeatureRepo; 

        public FeatureService(IFeatureRepository featureRepo, IRoomClassFeatureRepository roomClassFeatureRepo)
        {
            _featureRepo = featureRepo;
            _roomClassFeatureRepo = roomClassFeatureRepo; 
        }

        public async Task<ServiceResponse<List<Feature>>> GetAllFeatures(BaseQueryObject queryObject)
        {
            var (features, total) = await _featureRepo.GetAllFeatures(queryObject);

            return new ServiceResponse<List<Feature>>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = features,
                Total = total,
                Took = features.Count,
            };
        }

        public async Task<ServiceResponse<Feature>> GetFeatureById(int featureId)
        {
            var feature = await _featureRepo.GetFeatureById(featureId);
            if (feature == null)
            {
                return new ServiceResponse<Feature>
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.FEATURE_NOT_FOUND,
                };
            }

            return new ServiceResponse<Feature>
            {
                Status = ResStatusCode.OK,
                Success = true,
                Data = feature,
            };
        }

        public async Task<ServiceResponse> CreateNewFeature(CreateUpdateFeatureDto createFeatureDto, int adminId)
        {
            var featureWithSameName = await _featureRepo.GetFeatureByName(createFeatureDto.Name);
            if (featureWithSameName != null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.CONFLICT,
                    Success = false,
                    Message = ErrorMessage.DUPLICATE_FEATURE_NAME,
                };
            }

            var newFeature = new Feature
            {
                Name = createFeatureDto.Name,
                CreatedById = adminId,
                CreatedAt = DateTime.UtcNow,
            };

            await _featureRepo.CreateNewFeature(newFeature);
            return new ServiceResponse
            {
                Status = ResStatusCode.CREATED,
                Success = true,
                Message = SuccessMessage.CREATE_FEATURE_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> UpdateFeature(int featureId, CreateUpdateFeatureDto updateFeatureDto)
        {
            var targetFeature = await _featureRepo.GetFeatureById(featureId);
            if (targetFeature == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.FEATURE_NOT_FOUND,
                };
            }

            // Kiểm tra nếu có feature trùng tên
            var featureWithSameName = await _featureRepo.GetFeatureByName(updateFeatureDto.Name);
            if (featureWithSameName != null && featureWithSameName.Id != featureId)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.CONFLICT,
                    Success = false,
                    Message = ErrorMessage.DUPLICATE_FEATURE_NAME,
                };
            }

            targetFeature.Name = updateFeatureDto.Name;
            

            await _featureRepo.UpdateFeature(targetFeature);
            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.UPDATE_FEATURE_SUCCESSFULLY,
            };
        }

        public async Task<ServiceResponse> DeleteFeature(int featureId)
        {
            var feature = await _featureRepo.GetFeatureById(featureId);
            if (feature == null)
            {
                return new ServiceResponse
                {
                    Status = ResStatusCode.NOT_FOUND,
                    Success = false,
                    Message = ErrorMessage.FEATURE_NOT_FOUND,
                };
            }

            foreach (var roomClassFeature in feature.RoomClassFeatures.ToList())
            {
                // Xóa RoomClassFeature
                await _roomClassFeatureRepo.DeleteRoomClassFeature(roomClassFeature);
            }

            await _featureRepo.DeleteFeature(feature);
            return new ServiceResponse
            {
                Status = ResStatusCode.OK,
                Success = true,
                Message = SuccessMessage.DELETE_FEATURE_SUCCESSFULLY,
            };
        }
    }
}
