using System;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodecLib.Hashing;
using Xrpl.BinaryCodecLib.Types;
using Xrpl.BinaryCodecLib.Util;
using System.Numerics;
using Xrpl.ClientLib.Exceptions;
using Xrpl.Models.Methods;
using static Xrpl.Models.Common.Common;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Globalization;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/xrpConversion.ts

namespace Xrpl.Utils
{
    public static partial class BigIntegerExtensions
    {
        // this have to be used for extend BigInteger
        public static String ToRadixString(this BigInteger value, int radix)
        {
            if (radix <= 1 || radix > 36)
                throw new ArgumentOutOfRangeException(nameof(radix));
            if (value == 0)
                return "0";

            bool negative = value < 0;

            if (negative)
                value = -value;

            StringBuilder sb = new StringBuilder();

            for (; value > 0; value /= radix)
            {
                int d = (int)(value % radix);

                sb.Append((char)(d < 10 ? '0' + d : 'A' - 10 + d));
            }

            return (negative ? "-" : "") + string.Concat(sb.ToString().Reverse());
        }
        // this have to be used for extend BigInteger
        public static String ToRadixString(this double value, int radix)
        {
            if (radix <= 1 || radix > 36)
                throw new ArgumentOutOfRangeException(nameof(radix));
            if (value == 0)
                return "0";

            bool negative = value < 0;

            if (negative)
                value = -value;

            StringBuilder sb = new StringBuilder();

            for (; value > 0; value /= radix)
            {
                int d = (int)(value % radix);

                sb.Append((char)(d < 10 ? '0' + d : 'A' - 10 + d));
            }

            return (negative ? "-" : "") + string.Concat(sb.ToString().Reverse());
        }
    }

    // XRP Conversion
    public static class XrpConversion
    {

        private static double DROPS_PER_XRP = 1000000.0;
        private static int MAX_FRACTION_LENGTH = 6;
        private static int BASE_TEN = 10;
        private static string SANITY_CHECK = "/ ^-?[0 - 9.] +$/u";

        /// <summary>
        /// Convert Drops to XRP.
        /// </summary>
        /// <param name="dropsToConvert"> Drops to convert to XRP. This can be a string, number, or BigNumber.</param>
        /// <returns
        public static string DropsToXrp(double dropsToConvert)
        {
            return DropsToXrp(dropsToConvert.ToString());
        }

        /// <summary>
        /// Convert Drops to XRP.
        /// </summary>
        /// <param name="dropsToConvert"> Drops to convert to XRP. This can be a string, number, or BigNumber.</param>
        /// <returns
        public static string DropsToXrp(string dropsToConvert)
        {
            /*
            * Converting to BigNumber and then back to string should remove any
            * decimal point followed by zeros, e.g. '1.00'.
            * Important: specify base BASE_10 to avoid exponential notation, e.g. '1e-7'.
            */
            Debug.WriteLine($"CONVERTING FROM: {dropsToConvert}");
            string drops = BigInteger.Parse(dropsToConvert, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowExponent).ToRadixString(BASE_TEN);
            Debug.WriteLine($"DROPS: {drops}");
            // check that the value is valid and actually a number
            if (!(dropsToConvert is string) && drops != null)
            {
                throw new ValidationError($"dropsToXrp: invalid value '{dropsToConvert}', should be a BigNumber or string - encoded number.");
            }

            // drops are only whole units
            if (drops.Contains("."))
            {
                throw new ValidationError("dropsToXrp: value '${drops}' has too many decimal places.");
            }

            /*
             * This should never happen; the value has already been
             * validated above. This just ensures BigNumber did not do
             * something unexpected.
             */
            //if (!SANITY_CHECK.exec(drops))
            //{
            //    throw new ValidationError(
            //        "dropsToXrp: failed sanity check -" +
            //        " value '${drops}'," +
            //        " does not match(^-?[0 - 9] +$)."
            //    );
            //}

            // TODO: SHOULD BE BASE 10
            return ((decimal)BigInteger.Parse(dropsToConvert, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowExponent) / (decimal)new BigInteger(DROPS_PER_XRP)).ToString();
            //return ((decimal)BigInteger.Parse(dropsToConvert, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture) / (decimal)new BigInteger(DROPS_PER_XRP)).ToString("F"+BASE_TEN).TrimEnd('0');
        }

        /// <summary>
        /// Convert Drops to XRP.
        /// </summary>
        /// <param name="dropsToConvert"> Drops to convert to XRP. This can be a string, number, or BigNumber.</param>
        /// <returns
        public static string XrpToDrops(double xrpToConvert)
        {
            return XrpToDrops(xrpToConvert.ToString());
        }

        /// <summary>
        /// Convert an amount in XRP to an amount in drops.
        /// </summary>
        /// <param name="xrpToConvert">Amount in XRP.</param>
        /// <returns Amount in drops.
        public static string XrpToDrops(string xrpToConvert)
        {
            // Important: specify base BASE_TEN to avoid exponential notation, e.g. '1e-7'.
            Debug.WriteLine($"CONVERTING FROM: {xrpToConvert}");
            // TODO: SHOULD BE BASE 10
            string xrp = decimal.Parse(xrpToConvert, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowExponent).ToString();
            Debug.WriteLine($"XRP: {xrp}");
            // check that the value is valid and actually a number
            if (!(xrpToConvert is string) && xrp != null)
            {
                throw new ValidationError($"xrpToConvert: invalid value '{xrpToConvert}', should be a BigInteger or string - encoded number.");
            }

            // drops are only whole units

            string[] components = xrp.TrimEnd('0').Split(".");
            Debug.WriteLine($"COMPONENTS: {components.Length}");
            if (components.Length > 2)
            {
                throw new ValidationError("xrpToDrops: failed sanity check - value '${xrp}' has too many decimal points.");
            }
            string fraction = components.Length > 1 && components[1] != null ? components[1] : "0";
            Debug.WriteLine($"FRACTION LENGTH: {fraction.Length}");
            if (fraction.Length > MAX_FRACTION_LENGTH)
            {
                throw new ValidationError($"xrpToDrops: value '{xrp}' has too many decimal places.");
            }
            Debug.WriteLine($"FRACTION: {fraction}");
            // TODO: SHOULD BE BASE 10
            return new BigInteger(decimal.Parse(xrpToConvert, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowExponent) * (decimal)new BigInteger(DROPS_PER_XRP)).ToString();
        }
    }
}