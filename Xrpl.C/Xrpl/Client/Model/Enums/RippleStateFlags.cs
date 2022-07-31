using System;

namespace xrpl_c.Xrpl.Client.Model.Enums
{
    [Flags]
    public enum RippleStateFlags
    {
        lsfLowReserve = 65536,
        lsfHighReserve = 131072,
        lsfLowAuth = 262144,
        lsfHighAuth = 524288,
        lsfLowNoRipple = 1048576,
        lsfHighNoRipple = 2097152,
        lsfLowFreeze = 4194304,
        lsfHighFreeze = 8388608
    }
}