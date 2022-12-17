using System;
using System.Diagnostics;
using System.Globalization;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/timeConversion.ts

namespace Xrpl.Utils
{
    public static class DateTimeUtils
    {
        private const int RIPPLE_EPOCH_DIFF = 0x386d4380;

        public static long RippleTimeToUnixTime(long rpepoch)
        {
            return (rpepoch + RIPPLE_EPOCH_DIFF) * 1000;
        }

        public static long UnixTimeToRippleTime(long timestamp)
        {
            return (long)Math.Round((decimal)timestamp / 1000) - RIPPLE_EPOCH_DIFF;
        }

        public static string RippleTimeToISOTime(long rippleTime)
        {
            var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(RippleTimeToUnixTime(rippleTime)).ToUniversalTime();
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        public static long ISOTimeToRippleTime(string iso8601)
        {
            var date = DateTime.ParseExact(iso8601, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture).ToUniversalTime();
            var milliseconds = (long)date.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            return UnixTimeToRippleTime(milliseconds);
        }

        public static long ISOTimeToRippleTime(DateTime date)
        {
            var milliseconds = (long)date.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            return UnixTimeToRippleTime(milliseconds);
        }
    }
}