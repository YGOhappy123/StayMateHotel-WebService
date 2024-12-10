using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using server.Dtos.Response;
using server.Dtos.Feature; // Import Dto cho Feature
using server.Models;
using server.Queries;

namespace server.Interfaces.Services
{
    public interface IFeatureService
    {
        Task<ServiceResponse<List<Feature>>> GetAllFeatures(BaseQueryObject queryObject);
        Task<ServiceResponse<Feature>> GetFeatureById(int featureId);
        Task<ServiceResponse> CreateNewFeature(CreateUpdateFeatureDto createFeatureDto, int adminId);
        Task<ServiceResponse> UpdateFeature(int featureId, CreateUpdateFeatureDto updateFeatureDto);
        Task<ServiceResponse> DeleteFeature(int featureId);
    }
}
