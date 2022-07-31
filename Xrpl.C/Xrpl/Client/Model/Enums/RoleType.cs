using System.Runtime.Serialization;

namespace xrpl_c.Xrpl.Client.Model.Enums
{
    public enum RoleType
    {
        [EnumMember(Value = "gateway")]
        Gateway,
        [EnumMember(Value = "user")]
        User
    }
}