using System.Runtime.Serialization;

namespace xrpl_c.Xrpl.Client.Model.Enums
{
    public enum LedgerIndexType
    {
        [EnumMember(Value = "current")]
        Current,
        [EnumMember(Value = "closed")]
        Closed,
        [EnumMember(Value = "validated")]
        Validated
    }
}