using System;

namespace Ripple.Core.Enums
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
        public int CompareTo(EnumItem other)
        {
            return Math.Sign(Ordinal - other.Ordinal);
        }
        public override string ToString()
        {
            return Name;
        }

        public static implicit operator int(EnumItem item)
        {
            return item.Ordinal;
        }

        public static implicit operator string(EnumItem item)
        {
            return item.Name;
        }

        public override int GetHashCode()
        {
            return Ordinal;
        }
    }
}
