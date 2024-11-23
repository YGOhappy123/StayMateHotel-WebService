using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Queries
{
    public class BaseQueryObject
    {
        public int? Skip { get; set; }
        public int? Limit { get; set; }
        public string? Sort { get; set; } = "{\"CreatedAt\": \"DESC\"}";
        public string? Filter { get; set; } = "{}";
    }
}
