using System.Runtime.Serialization;

namespace xrpl_c.Xrpl.Client.Model.Enums
{
    public enum ServerState
    {
        [EnumMember(Value = "disconnected")]
        Disconnected,
        [EnumMember(Value = "connected")]
        Connected,
        [EnumMember(Value = "syncing")]
        Syncing,
        [EnumMember(Value = "tracking")]
        Tracking,
        [EnumMember(Value = "full")]
        Full,
        [EnumMember(Value = "validating")]
        Validating,
        [EnumMember(Value = "proposing")]
        Proposing
    }
}