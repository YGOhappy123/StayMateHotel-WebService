using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

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
        public const int UNPROCESSABLE_ENTITY = 422;
        public const int INTERNAL_SERVER_ERROR = 500;
    }
}
