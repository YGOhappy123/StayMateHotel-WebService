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
        public const string INCORRECT_USERNAME_OR_PASSWORD = "INCORRECT_USERNAME_OR_PASSWORD";
        public const string GOOGLE_AUTH_FAILED = "GOOGLE_AUTH_FAILED";
        public const string ROOM_NOT_FOUND = "ROOM_NOT_FOUND";
        public const string INVALID_CREDENTIALS = "INVALID_CREDENTIALS";
        public const string DATA_VALIDATION_FAILED = "DATA_VALIDATION_FAILED";
        public const string UPLOAD_IMAGE_FAILED = "UPLOAD_IMAGE_FAILED";
        public const string DELETE_IMAGE_FAILED = "DELETE_IMAGE_FAILED";
        public const string NO_PERMISSION = "NO_PERMISSION";
        public const string FLOOR_NOT_FOUND = "FLOOR_NOT_FOUND";
        public const string DUPLICATE_FLOOR_NAME = "DUPLICATE_FLOOR_NAME";
        public const string FLOOR_NOT_FOUND_OR_UNAVAILABLE = "FLOOR_NOT_FOUND_OR_UNAVAILABLE";
    }
}
