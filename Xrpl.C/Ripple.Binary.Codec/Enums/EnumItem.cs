using System;

namespace Ripple.Binary.Codec.Enums
{
    public class EnumItem : IComparable<EnumItem>
    {
        public readonly int Ordinal;
        public readonly string Name;
        public EnumItem(string name, int ordinal)
        {
            Ordinal = ordinal;
            Name = name;
        }
        public int CompareTo(EnumItem other) => Math.Sign(Ordinal - other.Ordinal);
        public override string ToString() => Name;

        public static implicit operator int(EnumItem item) => item.Ordinal;

        public static implicit operator string(EnumItem item) => item.Name;

        public override int GetHashCode() => Ordinal;
    }
}
