using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using server.Models;
using server.Queries;

namespace server.Interfaces.Repositories
{
    public interface IFeatureRepository
    {
        Task<(List<Feature>, int)> GetAllFeatures(BaseQueryObject queryObject);
        Task<Feature?> GetFeatureById(int featureId);
        Task<Feature?> GetFeatureByName(string featureName);
        Task CreateNewFeature(Feature feature);
        Task UpdateFeature(Feature feature);
        Task DeleteFeature(Feature feature);
       
    }
}
