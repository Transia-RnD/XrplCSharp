using System;

namespace xrpl_c.Xrpl.Client.Model.Enums
{
    [Flags]
    public enum PaymentChannelClaimFlags : uint
    {
        tfRenew = 65536,
        tfClose = 131072
    }
}