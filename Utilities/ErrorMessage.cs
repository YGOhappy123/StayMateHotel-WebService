using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Utilities
{
    public static class ErrorMessage
    {
        public const string USERNAME_EXISTED = "USERNAME_EXISTED";
        public const string USER_NOT_FOUND = "USER_NOT_FOUND";
        public const string ROOM_NOT_FOUND = "ROOM_NOT_FOUND";
        public const string INVALID_CREDENTIALS = "INVALID_CREDENTIALS";
        public const string DATA_VALIDATION_FAILED = "DATA_VALIDATION_FAILED";
    }
}
