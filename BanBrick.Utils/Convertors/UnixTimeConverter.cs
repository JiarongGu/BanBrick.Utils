using System;

namespace BanBrick.Utils.Convertors
{
    public static class UnixTimeConverter
    {
        private static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts a DateTime to a Unix timestamp
        /// </summary>
        public static int ToUnixTimestamp(DateTime dateTime)
        {
            return (int)(dateTime.ToUniversalTime() - epoch).TotalSeconds;
        }
    }
}
