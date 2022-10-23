

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/utils/index.ts

using System;
using System.Text.RegularExpressions;

namespace Xrpl.Models.Utils //todo ?
{
    internal static class Index
    {
        /// <summary>
        /// Perform bitwise AND (&) to check if a flag is enabled within Flags (as a number).
        /// </summary>
        /// <param name="Flags"> A number that represents flags enabled.</param>
        /// <param name="checkFlag">A specific flag to check if it's enabled within Flags.</param>
        /// <returns>True if checkFlag is enabled within Flags.</returns>
        public static bool IsFlagEnabled(this uint Flags, uint checkFlag)
        {
            // eslint-disable-next-line no-bitwise -- flags needs bitwise
            return (checkFlag & Flags) == checkFlag;
        }
        /// <summary>
        /// Check if string is in hex format.
        /// </summary>
        /// <param name="str"> The string to check if it's in hex format.</param>
        /// <returns>True if string is in hex format</returns>
        public static bool IsHex(this string str) => System.Text.RegularExpressions.Regex.IsMatch(str, @"^[0-9A-Fa-f]+$");
    }
}
