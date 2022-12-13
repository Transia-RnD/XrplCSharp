using System;
using System.Diagnostics;

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
            return new DateTime(RippleTimeToUnixTime(rippleTime)).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        public static long ISOTimeToRippleTime(string iso8601)
        {
            return UnixTimeToRippleTime(DateTime.Parse(iso8601).Ticks / TimeSpan.TicksPerMillisecond);
        }
    }
}