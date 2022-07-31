using System;

namespace xrpl_c.Xrpl.Client.Model.Enums
{
    [Flags]
    public enum TrustSetFlags : uint
    {
        tfSetfAuth = 65536,
        tfSetNoRipple = 131072,
        tfClearNoRipple = 262144,
        tfSetFreeze = 1048576,
        tfClearFreeze = 2097152
    }
}