using System.Runtime.Serialization;

namespace xrpl_c.Xrpl.Client.Model.Enums
{
    public enum LedgerEntryRequestType
    {
        [EnumMember(Value = "index")]
        Index,
        [EnumMember(Value = "account_root")]
        AccountRoot,
        [EnumMember(Value = "directory")]
        Directory,
        [EnumMember(Value = "offer")]
        Offer,
        [EnumMember(Value = "ripple_state")]
        RippleState
    }
}