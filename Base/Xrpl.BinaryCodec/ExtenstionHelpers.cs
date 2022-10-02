using System.Text.RegularExpressions;

namespace Xrpl.BinaryCodecLib
{
    public static class ExtenstionHelpers
    {
        public class NumFunc
        {
            public int Precision { get; set; }
            public int Exponent { get; set; }
            public ulong Mantissa { get; set; }
        }
        public static NumFunc NumFuncAll(this string input)
        {
            string regex = @"^([-+])?(\d+)?(\.(\d+))?([eE]([+-]?\d+))?$";
            var match = Regex.Match(input, regex);
            if (!match.Success)
            {
                throw new BinaryCodecException($"Value {input} must match {regex}");
            }
            var numberGroup = match.Groups[2];
            var fractionGroup = match.Groups[4];
            var exponentGroup = match.Groups[6];
            var exponent = 0;
            var mantissaString = numberGroup.Success ? numberGroup.Value : "";
            if (fractionGroup.Success)
            {
                var fraction = fractionGroup.Value;
                mantissaString += fraction;
                exponent -= fraction.Length;
            }
            if (exponentGroup.Success)
            {
                exponent += int.Parse(exponentGroup.Value);
            }

            mantissaString = mantissaString.TrimStart('0');
            var trimmed = mantissaString.TrimEnd('0');
            if (trimmed.Length == 0)
            {
                trimmed = "0";
                mantissaString = "0";
            }
            return new NumFunc()
            {
                Precision = trimmed.Length,
                Exponent = mantissaString.Length - trimmed.Length,
                Mantissa = ulong.Parse(trimmed)
            };
        }
        public static int Exponent(this string input)
        {
            string regex = @"^([-+])?(\d+)?(\.(\d+))?([eE]([+-]?\d+))?$";
            var match = Regex.Match(input, regex);
            if (!match.Success)
            {
                throw new BinaryCodecException($"Value {input} must match {regex}");
            }
            var numberGroup = match.Groups[2];
            var fractionGroup = match.Groups[4];
            var exponentGroup = match.Groups[6];
            var exponent = 0;
            var mantissaString = numberGroup.Success ? numberGroup.Value : "";
            if (fractionGroup.Success)
            {
                var fraction = fractionGroup.Value;
                mantissaString += fraction;
                exponent -= fraction.Length;
            }
            if (exponentGroup.Success)
            {
                exponent += int.Parse(exponentGroup.Value);
            }
            mantissaString = mantissaString.TrimStart('0');
            var trimmed = mantissaString.TrimEnd('0');
            if (trimmed.Length == 0)
            {
                trimmed = "0";
            }
            return mantissaString.Length - trimmed.Length;
        }

        public static int Precision(this string input)
        {
            string regex = @"^([-+])?(\d+)?(\.(\d+))?([eE]([+-]?\d+))?$";
            var match = Regex.Match(input, regex);
            if (!match.Success)
            {
                throw new BinaryCodecException($"Value {input} must match {regex}");
            }
            var numberGroup = match.Groups[2];
            var fractionGroup = match.Groups[4];
            var mantissaString = numberGroup.Success ? numberGroup.Value : "";
            if (fractionGroup.Success)
            {
                var fraction = fractionGroup.Value;
                mantissaString += fraction;
            }
            mantissaString = mantissaString.TrimStart('0');
            var trimmed = mantissaString.TrimEnd('0');
            if (trimmed.Length == 0)
            {
                trimmed = "0";
            }
            return trimmed.Length;
        }
    }
}

