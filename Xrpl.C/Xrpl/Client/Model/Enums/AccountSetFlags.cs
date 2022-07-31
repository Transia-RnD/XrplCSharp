namespace xrpl_c.Xrpl.Client.Model.Enums
{
    public enum AccountSetFlags
    {
        asfRequireDest = 1,
        asfRequireAuth = 2,
        asfDisallowXRP = 3,
        asfDisableMaster = 4,
        asfAccountTxnID = 5,
        asfNoFreeze = 6,
        asfGlobalFreeze = 7,
        asfDefaultRipple = 8
    }
}