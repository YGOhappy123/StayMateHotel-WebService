using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetStartOfWeek(this DateTime timestamp)
        {
            int diff = (7 + (int)timestamp.DayOfWeek - (int)DayOfWeek.Monday) % 7;
            return timestamp.AddDays(-diff);
        }

        public static DateTime GetEndOfDay(this DateTime timestamp)
        {
            return timestamp.Date.AddDays(1).AddSeconds(-1);
        }

        public static DateTime GetEndOfWeek(this DateTime timestamp)
        {
            int diff = 7 - (int)timestamp.DayOfWeek;
            return timestamp.AddDays(diff);
        }

        public static DateTime GetEndOfMonth(this DateTime timestamp)
        {
            int nextMonth = timestamp.Month == 12 ? 1 : timestamp.Month + 1;
            int nextMonthYear = timestamp.Month == 12 ? timestamp.Year + 1 : timestamp.Year;
            DateTime firstDayOfNextMonth = new DateTime(nextMonthYear, nextMonth, 1, timestamp.Hour, timestamp.Minute, timestamp.Second);

            return firstDayOfNextMonth.AddDays(-1);
        }

        public static int GetDaysInMonth(this DateTime timestamp)
        {
            return DateTime.DaysInMonth(timestamp.Year, timestamp.Month);
        }

        public static DateTime AddByUnitAndAmount(this DateTime timestamp, int amount, string unit)
        {
            return unit.ToLower() switch
            {
                "hour" => timestamp.AddHours(amount),
                "day" => timestamp.AddDays(amount),
                "month" => timestamp.AddMonths(amount),
                "year" => timestamp.AddYears(amount),
                _ => throw new ArgumentException("Invalid type"),
            };
        }
    }
}
