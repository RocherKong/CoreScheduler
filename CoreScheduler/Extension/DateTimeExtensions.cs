using CoreScheduler.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreScheduler.Extension
{
    public static class DateTimeExtensions
    {
        internal static DateTime First(this DateTime current)
        {
            return current.AddDays(1 - current.Day);
        }

        internal static DateTime FirstOfYear(this DateTime current)
        {
            return new DateTime(current.Year, 1, 1);
        }

        /// <summary>
        /// last day of current Month
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        internal static DateTime Last(this DateTime current)
        {
            var daysInMonth = DateTime.DaysInMonth(current.Year, current.Month);
            return current.First().AddDays(daysInMonth - 1);
        }

        /// <summary>
        /// Last Special Weekday of Current Month.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="dayofweek"></param>
        /// <returns></returns>
        internal static DateTime Last(this DateTime current, DayOfWeek dayofweek)
        {
            var last = current.Last();
            var diff = dayofweek - last.DayOfWeek;
            if (diff > 0)
            {
                diff -= 7;
            }
            return last.AddDays(diff);
        }

        internal static DateTime ThisOrNext(this DateTime current, DayOfWeek dayofweek)
        {
            var offsetDays = dayofweek - current.DayOfWeek;
            if (offsetDays < 0)
            {
                offsetDays += 7;
            }
            return current.AddDays(offsetDays);
        }

        internal static DateTime Next(this DateTime current, DayOfWeek dayofweek)
        {
            var offsetDays = dayofweek - current.DayOfWeek;
            if (offsetDays <= 0)
            {
                offsetDays += 7;
            }
            return current.AddDays(offsetDays);
        }

        internal static bool IsWeekday(this DateTime current)
        {
            switch (current.DayOfWeek)
            {

                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// next weekday after duration days.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="Duration"></param>
        /// <returns></returns>
        internal static DateTime NextWeekDay(this DateTime current, int Duration)
        {
            while (Duration >= 1)
            {
                Duration--;
                current = current.AddDays(1);
                while (!current.IsWeekday())
                {
                    current = current.AddDays(1);
                }
            }
            return current;
        }

        internal static DateTime ClearMinutesAndSeconds(this DateTime current)
        {
            return current.AddMinutes(-1 * current.Minute)
                .AddSeconds(-1 * current.Second)
                .AddMilliseconds(-1 * current.Millisecond);
        }

        /// <summary>
        /// Compute Date of which num Week
        /// </summary>
        /// <param name="current"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        internal static DateTime ToWeek(this DateTime current, Week week)
        {
            switch (week)
            {
                case Week.First:
                    return current.First();
                case Week.Second:
                    return current.First().AddDays(7);
                case Week.Third:
                    return current.First().AddDays(14);
                case Week.Fourth:
                    return current.First().AddDays(21);
                case Week.Last:
                    return current.Last().AddDays(-7);
                default:
                    return current.First();
            }
        }
    }
}
