using System;

namespace xrpl_c.Xrpl.Client.Model.Enums
{
    [Flags]
    public enum AccountRootFlags
    {
        lsfPasswordSpent = 65536,
        lsfRequireDestTag = 131072,
        lsfRequireAuth = 262144,
        lsfDisallowXRP = 524288,
        lsfDisableMaster = 1048576,
        lsfNoFreeze = 2097152,
        lsfGlobalFreeze = 4194304,
        lsfDefaultRipple = 8388608
    }
}