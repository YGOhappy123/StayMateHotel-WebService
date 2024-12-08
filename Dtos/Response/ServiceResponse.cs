using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.Response
{
    public class ServiceResponse
    {
        public int Status { get; set; } = 200;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public T? Data { get; set; }
        public int? Total { get; set; }
        public int? Took { get; set; }
    }
}
