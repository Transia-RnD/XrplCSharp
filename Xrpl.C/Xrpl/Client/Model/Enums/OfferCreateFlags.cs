using System;

namespace xrpl_c.Xrpl.Client.Model.Enums
{
    [Flags]
    public enum OfferCreateFlags : uint
    {
        tfPassive = 65536,
        tfImmediateOrCancel = 131072,
        tfFillOrKill = 262144,
        tfSell = 524288
    }
}