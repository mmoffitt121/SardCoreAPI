using System.Runtime.CompilerServices;

namespace SardCoreAPI.Models.DateTime
{
    public static class DateTimeExtensions
    {
        private static readonly int SECONDS_PER_MINUTE = 64;
        private static readonly int MINUTES_PER_HOUR = 64;
        private static readonly int HOURS_PER_DAY = 32;
        private static readonly int DAYS_PER_MONTH = 32;
        private static readonly int MONTHS_PER_YEAR = 16;
    }
}
