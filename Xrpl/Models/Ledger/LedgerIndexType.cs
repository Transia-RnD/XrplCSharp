using System.Runtime.Serialization;

// https://xrpl.org/ledgers.html#open-closed-and-validated-ledgers
//https://github.com/XRPLF/xrpl.js/blob/76b73e16a97e1a371261b462ee1a24f1c01dbb0c/packages/xrpl/src/models/common/index.ts

namespace Xrpl.Models.Ledger;

/// <summary>
/// The rippled server makes a distinction between ledger versions that are open, closed, and validated.<br/>
/// A server has one open ledger, any number of closed but unvalidated ledgers, and an immutable history of validated ledgers.
/// </summary>
public enum LedgerIndexType
{
    [EnumMember(Value = "current")]
    Current,

    [EnumMember(Value = "closed")]
    Closed,

    [EnumMember(Value = "validated")]
    Validated
}