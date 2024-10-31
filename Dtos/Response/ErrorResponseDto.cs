using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dtos.Response
{
    public class ErrorResponseDto
    {
        public int Status { get; set; } = 500;
        public string? Message { get; set; }
        public object? Error { get; set; }
    }
}
