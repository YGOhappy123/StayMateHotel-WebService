using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Utilities
{
    public static class ResStatusCode
    {
        public const int OK = 200;
        public const int CREATED = 201;
        public const int BAD_REQUEST = 400;
        public const int UNAUTHORIZED = 401;
        public const int NOT_FOUND = 404;
        public const int CONFLICT = 409;
    }
}
