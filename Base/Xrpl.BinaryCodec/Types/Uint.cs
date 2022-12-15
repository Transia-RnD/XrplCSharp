using System;
using System.Globalization;
using System.Numerics;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodec.Serdes;
using static Xrpl.BinaryCodec.Types.SerializedType;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/uint.ts

namespace Xrpl.BinaryCodec.Types
{
    public class UInt : Comparable
    {
        protected static int width;

        /// <summary>
        /// Compares two numbers
        /// </summary>
        /// <param name="n1">First number</param>
        /// <param name="n2">Second number</param>
        /// <returns>-1 if n1 is less than n2, 0 if n1 is equal to n2, 1 if n1 is greater than n2</returns>
        public static int Compare(BigInteger n1, BigInteger n2)
        {
            return n1 < n2 ? -1 : n1 == n2 ? 0 : 1;
        }

        public UInt(byte[] bytes) : base(bytes)
        {
        }

        public int CompareTo(UInt other)
        {
            return Compare((BigInteger)this.ValueOf, (BigInteger)other.ValueOf);
        }

        public object ToJSON()
        {
            var val = this.ValueOf;
            return val is int ? val : val.ToString();
        }

        public object ValueOf { get; }
    }
}