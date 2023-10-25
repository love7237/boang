using System;
using System.Linq;

namespace T.Utility.Extensions
{
    /// <summary>
    /// DateTime extension methods for common scenarios.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Returns the number of seconds that have elapsed since 1970-01-01T00:00:00Z.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixTimeSeconds(this DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        }

        /// <summary>
        /// Returns the number of milliseconds that have elapsed since 1970-01-01T00:00:00.000Z.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixTimeMilliseconds(this DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Converts a Unix time expressed as the number of seconds that have elapsed since 1970-01-01T00:00:00Z to a System.DateTimeOffset value.
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeSeconds(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
        }

        /// <summary>
        /// Converts a Unix time expressed as the number of milliseconds that have elapsed since 1970-01-01T00:00:00Z to a System.DateTimeOffset value.
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeMilliseconds(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(unixTime).LocalDateTime;
        }

        /// <summary>
        /// Returns the maximum DateTime in a generic sequence.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DateTime Max(params DateTime[] list)
        {
            if (list == null || list.Length == 0)
                throw new ArgumentNullException("The sequence of values is empty");

            return list.Max();
        }

        /// <summary>
        /// Returns the minimum value in a generic sequence.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DateTime Min(params DateTime[] list)
        {
            if (list == null || list.Length == 0)
                throw new ArgumentNullException("The sequence of values is empty");

            return list.Min();
        }
    }
}
