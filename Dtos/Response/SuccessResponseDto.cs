using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.Response
{
    public class SuccessResponseDto
    {
        public int Status { get; set; } = 200;
        public int? Total { get; set; }
        public int? Took { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
