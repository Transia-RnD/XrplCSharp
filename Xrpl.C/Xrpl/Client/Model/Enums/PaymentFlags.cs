using System;

namespace xrpl_c.Xrpl.Client.Model.Enums
{
    [Flags]
    public enum PaymentFlags : uint
    {
        tfNoDirectRipple = 65536,
        tfPartialPayment = 131072,
        tfLimitQuality = 262144,
    }
}