using System;

namespace xrpl_c.Xrpl.Client.Model.Enums
{
    [Flags]
    public enum OfferFlags
    {
        lsfPassive = 65536,
        lsfSell = 131072
    }
}