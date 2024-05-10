using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Xrpl.BinaryCodec.Tests
{
    public static class ExtensionHelpers
    {
        public static string Repeat(this string s, int n)
            => new StringBuilder(s.Length * n).Insert(0, s, n).ToString();

        public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }

        public static bool StrictDictEqual(Dictionary<string, dynamic> dic1, Dictionary<string, dynamic> dic2)
        {
            return dic1.OrderBy(r => r.Key).SequenceEqual(dic2.OrderBy(r => r.Key));
        }
    }
}