using System;

namespace xrpl_c.Xrpl.Client.Model.Enums
{
    [Flags]
    public enum NFTokenMintFlags : uint
    {
        tfBurnable = 1,
        tfOnlyXRP = 2,
        tfTrustLine = 3,
        tfTransferable = 4
    }
}