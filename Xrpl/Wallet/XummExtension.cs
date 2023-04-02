using System;
using System.Collections.Generic;
using System.Linq;

namespace Xrpl.Wallet
{
    public static class XummExtension
    {
        /// <summary>
        /// generate Entropy from xumm numbers
        /// </summary>
        /// <param name="numbers">xumm numbers</param>
        /// <returns>byte[] Entropy</returns>
        /// <exception cref="ArgumentException">when wrong has wrong digits</exception>
        public static byte[] EntropyFromXummNumbers(string[] numbers)
        {
            if (!CheckXummNumbers(numbers))
                throw new ArgumentException("Wrong numbers");

            var vals = numbers.Select(x => $"0000{int.Parse(x.Substring(0, 5)):X}"[^4..]).ToArray();

            var buffer = new List<byte>();
            foreach (var val in vals)
            {
                var v = Enumerable.Range(0, val.Length)
                   .Where(x => x % 2 == 0)
                   .Select(x => Convert.ToByte(val.Substring(x, 2), 16))
                   .ToArray();
                buffer.AddRange(v);

            }
            return buffer.ToArray();
        }

        /// <summary>
        /// xumm numbers validation
        /// </summary>
        /// <param name="numbers">xum numbers</param>
        /// <returns></returns>
        public static bool CheckXummNumbers(string[] numbers) => numbers.Select((n, i) => CheckXummSum(i, n)).All(c => c);

        /// <summary>
        /// xumm validation for part od numbers
        /// </summary>
        /// <param name="position">numbers position</param>
        /// <param name="number">xum numbers</param>
        /// <returns></returns>
        public static bool CheckXummSum(int position, string number)
        {
            if (number.Length != 6)
                return false;

            var checkSum = int.Parse(number[5..]);
            var value = int.Parse(number[..5]);
            var sum = value * (position * 2 + 1) % 9;
            return sum == checkSum;
        }

    }
}
