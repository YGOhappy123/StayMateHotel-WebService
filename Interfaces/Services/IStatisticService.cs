using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Dtos.Response;

namespace server.Interfaces.Services
{
    public interface IStatisticService
    {
        Task<ServiceResponse<object>> GetSummaryStatistic(string type);
        Task<ServiceResponse<object>> GetPopularStatistic(string type);
        Task<ServiceResponse<object>> GetRevenuesChart(string type, string locale);
    }
}
