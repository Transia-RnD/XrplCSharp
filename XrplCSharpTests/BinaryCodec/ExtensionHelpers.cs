using System;
using System.Text;

namespace XrplTests.BinaryCodecTests
{
    public static class ExtensionHelpers
    {
        public static string Repeat(this string s, int n)
            => new StringBuilder(s.Length * n).Insert(0, s, n).ToString();
    }
}

