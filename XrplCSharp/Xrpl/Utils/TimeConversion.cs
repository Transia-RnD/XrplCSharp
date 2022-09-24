﻿using System;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/timeConversion.ts

namespace XrplCSharp.Xrpl.Utils
{
    public static class TimeConversion
    {
        static uint RIPPLE_EPOCH_DIFF = 0x386d4380;

        public static long RippleTimeToUnixTime(int rpepoch)
        {
            return (rpepoch + RIPPLE_EPOCH_DIFF) * 1000;
        }

        public static long UnixTimeToRippleTime(int timestamp)
        {
            return Math.Round(timestamp / 1000) - RIPPLE_EPOCH_DIFF;
        }

        public static long RippleTimeToISOTime(int rippleTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(RippleTimeToUnixTime(rippleTime)).ToString("o");
        }
    }
}
