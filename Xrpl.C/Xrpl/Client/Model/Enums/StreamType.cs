using System.Runtime.Serialization;

namespace xrpl_c.Xrpl.Client.Model.Enums
{
    public enum StreamType
    {
        [EnumMember(Value = "ledgerClosed")]
        Ledger,
        [EnumMember(Value = "validationReceived")]
        Validations,
        [EnumMember(Value = "transaction")]
        Transaction,
        [EnumMember(Value = "peerStatusChange")]
        PeerStatus,
        [EnumMember(Value = "transaction")]
        OrderBook
    }
}