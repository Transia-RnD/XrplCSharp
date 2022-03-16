using System;
using System.Linq;
using System.Text;
using System.Web;

namespace RippleDotNet.Extensions
{
    public static class ExtensionHelpers
    {
        public static string ToHex(this string input)
        {
            char[] values = input.ToCharArray();

            StringBuilder sb = new StringBuilder();

            foreach (char letter in values)
            {
                int value = Convert.ToInt32(letter);
                sb.Append(string.Format("{0:X}", value));                
            }

            return sb.ToString();
        }

        public static string FromHexString(this string input)
        {
            byte[] bytes = Enumerable.Range(0, input.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(input.Substring(x, 2), 16))
                .ToArray();
            return HttpUtility.HtmlEncode(Encoding.ASCII.GetString(bytes));
        }
    }
}
