using System.Globalization;
using System.Threading;

namespace xrpl_c.Xrpl.Client.Extensions
{
    public static class TrustlineExtensions
    {
        /// <summary>
        /// overrides the value to the maximum allowed for trustline installation if it greater that maximum
        /// </summary>
        /// <param name="value">value for the installation</param>
        /// <returns>valid value</returns>
        public static string ToScientfic(this string value)
        {
            var num = double.Parse(value, (NumberStyles.Float & NumberStyles.AllowExponent)| NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            var valid = $"{num:################e00}";
            return valid;
        }
    }
}
