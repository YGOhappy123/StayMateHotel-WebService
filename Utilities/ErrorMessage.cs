using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Utilities
{
    public static class ErrorMessage
    {
        public const string USERNAME_EXISTED = "USERNAME_EXISTED";
        public const string EMAIL_EXISTED = "EMAIL_EXISTED";
        public const string PHONE_NUMBER_EXISTED = "PHONE_NUMBER_EXISTED";
        public const string USER_NOT_FOUND = "USER_NOT_FOUND";
        public const string INCORRECT_USERNAME_OR_PASSWORD = "INCORRECT_USERNAME_OR_PASSWORD";
        public const string GOOGLE_AUTH_FAILED = "GOOGLE_AUTH_FAILED";
        public const string ROOM_NOT_FOUND = "ROOM_NOT_FOUND";
        public const string ROOM_NOT_FOUND_OR_UNAVAILABLE = "ROOM_NOT_FOUND_OR_UNAVAILABLE";
        public const string ROOM_CANNOT_BE_UPDATED = "ROOM_CANNOT_BE_UPDATED";
        public const string ROOM_IS_RESERVED_AND_WILL_BE_OCCUPIED_SOON = "ROOM_IS_RESERVED_AND_WILL_BE_OCCUPIED_SOON";
        public const string ROOM_CANNOT_BE_DELETED = "ROOM_CANNOT_BE_DELETED";
        public const string DUPLICATE_ROOM_NUMBER = "DUPLICATE_ROOM_NUMBER";
        public const string BOOKING_NOT_FOUND = "BOOKING_NOT_FOUND";
        public const string CANNOT_UPDATE_BOOKING_WITH_THIS_STATUS = "CANNOT_UPDATE_BOOKING_WITH_THIS_STATUS";
        public const string PLEASE_CLEAN_THE_ROOMS_FIRST = "PLEASE_CLEAN_THE_ROOMS_FIRST";
        public const string INVALID_DATE_OR_NO_DEPOSIT_TRACKED = "INVALID_DATE_OR_NO_DEPOSIT_TRACKED";
        public const string INVALID_CREDENTIALS = "INVALID_CREDENTIALS";
        public const string DATA_VALIDATION_FAILED = "DATA_VALIDATION_FAILED";
        public const string UPLOAD_IMAGE_FAILED = "UPLOAD_IMAGE_FAILED";
        public const string DELETE_IMAGE_FAILED = "DELETE_IMAGE_FAILED";
        public const string NO_PERMISSION = "NO_PERMISSION";
        public const string FEATURE_NOT_FOUND = "FEATURE_NOT_FOUND";
        public const string DUPLICATE_FEATURE_NAME = "DUPLICATE_FEATURE_NAME";
        public const string ROOM_CLASS_NOT_FOUND = "ROOM_CLASS_NOT_FOUND";
        public const string DUPLICATE_ROOM_CLASS_NAME = "DUPLICATE_ROOM_CLASS_NAME";
        public const string ROOM_CLASS_CANNOT_BE_DELETED = "ROOM_CLASS_CANNOT_BE_DELETED";
        public const string FLOOR_NOT_FOUND = "FLOOR_NOT_FOUND";
        public const string DUPLICATE_FLOOR_NUMBER = "DUPLICATE_FLOOR_NUMBER";
        public const string FLOOR_CANNOT_BE_DELETED = "FLOOR_CANNOT_BE_DELETED";

        public const string SERVICE_NOT_FOUND = "SERVICE_NOT_FOUND";
        public const string DUPLICATE_SERVICE_NAME = "DUPLICATE_SERVICE_NAME";
        public const string SERVICE_NOT_FOUND_OR_UNAVAILABLE = "SERVICE_NOT_FOUND_OR_UNAVAILABLE";
    }
}
