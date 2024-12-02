using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using server.Extensions;

namespace server.Utilities
{
    public class TimestampHandler
    {
        public static DateTime GetStartOfTimeByType(DateTime timestamp, string type)
        {
            return type.ToLower() switch
            {
                "daily" => timestamp.Date,
                "weekly" => timestamp.Date.GetStartOfWeek(),
                "monthly" => new DateTime(timestamp.Year, timestamp.Month, 1),
                "yearly" => new DateTime(timestamp.Year, 1, 1),
                _ => throw new ArgumentException("Invalid type"),
            };
        }

        public static DateTime GetEndOfTimeByType(DateTime timestamp, string type)
        {
            return type.ToLower() switch
            {
                "daily" => timestamp.GetEndOfDay(),
                "weekly" => timestamp.GetEndOfWeek().GetEndOfDay(),
                "monthly" => timestamp.GetEndOfMonth().GetEndOfDay(),
                "yearly" => new DateTime(timestamp.Year, 12, 31).GetEndOfDay(),
                _ => throw new ArgumentException("Invalid type"),
            };
        }

        public static DateTime GetPreviousTimeByType(DateTime timestamp, string type)
        {
            return type.ToLower() switch
            {
                "daily" => timestamp.AddDays(-1),
                "weekly" => timestamp.AddDays(-7),
                "monthly" => timestamp.AddMonths(-1),
                "yearly" => timestamp.AddYears(-1),
                _ => throw new ArgumentException("Invalid type"),
            };
        }

        public static DateTime GetNow()
        {
            return DateTime.Now;
        }

        public static bool IsSame(DateTime timestamp1, DateTime timestamp2, string compareUnit)
        {
            return compareUnit.ToLower() switch
            {
                "hour" => timestamp1.Date == timestamp2.Date && timestamp1.Hour == timestamp2.Hour,
                "day" => timestamp1.Date == timestamp2.Date,
                "week" => timestamp1.Date.GetStartOfWeek() == timestamp2.Date.GetStartOfWeek(),
                "month" => timestamp1.Year == timestamp2.Year && timestamp1.Month == timestamp2.Month,
                "year" => timestamp1.Year == timestamp2.Year,
                _ => throw new ArgumentException("Invalid type"),
            };
        }
    }
}
