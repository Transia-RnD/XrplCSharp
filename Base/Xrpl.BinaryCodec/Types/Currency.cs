using System;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Util;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/currency.ts
//https://xrpl.org/currency-formats.html#currency-formats

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Class defining how to encode and decode Currencies
    /// </summary>
    public class Currency : Hash160
    {
        /// <summary>
        ///  ISO code of this currency
        /// </summary>
        public readonly string IsoCode;
        /// <summary>
        /// Test if this amount is in units of Native Currency(XRP)
        /// </summary>
        public readonly bool IsNative;
        /// <summary>
        /// Native XRP Currency
        /// </summary>
        public static readonly Currency Xrp = new Currency(new byte[20]);
        /// <summary>
        /// Constructs a Currency object
        /// </summary>
        /// <param name="buffer">bytes buffer</param>
        public Currency(byte[] buffer) : base(buffer)
        {
            IsoCode = GetCurrencyCodeFromTlcBytes(buffer, out IsNative);
        }
        /// <summary>
        /// get currency code from bytes
        /// </summary>
        /// <param name="bytes">bytes</param>
        /// <param name="isNative">will true if currency is XRP</param>
        /// <returns></returns>
        public static string GetCurrencyCodeFromTlcBytes(byte[] bytes, out bool isNative)
        {
            int i;
            var zeroInNonCurrencyBytes = true;
            var allZero = true;

            for (i = 0; i < 20; i++)
            {
                allZero = allZero && bytes[i] == 0;
                zeroInNonCurrencyBytes = zeroInNonCurrencyBytes && 
                    (i == 12 || i == 13 || i == 14 || bytes[i] == 0); 
            }
            if (allZero)
            {
                isNative = true;
                return "XRP";
            }
            if (zeroInNonCurrencyBytes)
            {
                isNative = false;
                return IsoCodeFromBytesAndOffset(bytes, 12);
            }
            isNative = false;
            return null;
        }

        private static char CharFrom(byte[] bytes, int i)
        {
            return (char)bytes[i];
        }
        /// <summary>
        /// Return the ISO code of this currency
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static string IsoCodeFromBytesAndOffset(byte[] bytes, int offset)
        {
            var a = CharFrom(bytes, offset);
            var b = CharFrom(bytes, offset + 1);
            var c = CharFrom(bytes, offset + 2);
            return "" + a + b + c;
        }
        /// <summary>
        /// decode currency from json field
        /// </summary>
        /// <param name="token">json field</param>
        /// <returns></returns>
        public new static Currency FromJson(JToken token)
        {
            return token == null ? null : FromString(token.ToString());
        }
        /// <summary>
        /// decode currency from string
        /// </summary>
        /// <param name="str">string currency code</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Currency FromString(string str)
        {
            if (str == "XRP")
            {
                return Xrp;
            }
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (str.Length)
            {
                case 40:
                    return new Currency(B16.Decode(str));
                case 3:
                    return new Currency(EncodeCurrency(str));
            }
            throw new InvalidOperationException(
                "Currency must either be a 3 letter iso code " +
                "or a 20 byte hash encoded in hexadecimal"
            );
        }

        /// <summary>
        /// The following are static methods, legacy from when there was no
        /// usage of Currency objects, just String with "XRP" ambiguity.
        /// </summary>
        /// <param name="currencyCode">currency code</param>
        /// <returns></returns>
        public static byte[] EncodeCurrency(string currencyCode)
        {
            byte[] currencyBytes = new byte[20];
            currencyBytes[12] = (byte)char.ConvertToUtf32(currencyCode, 0);
            currencyBytes[13] = (byte)char.ConvertToUtf32(currencyCode, 1);
            currencyBytes[14] = (byte)char.ConvertToUtf32(currencyCode, 2);
            return currencyBytes;
        }

        public static implicit operator Currency(string v)
        {
            return FromString(v);
        }
        public static implicit operator Currency(JToken v)
        {
            return FromJson(v);
        }
        public static implicit operator JToken(Currency v)
        {
            return v.ToString();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (IsoCode != null)
            {
                return IsoCode;
            }
            return base.ToString();
        }
        /// <summary>
        /// Defines how to read a Currency from a BinaryParser
        /// </summary>
        /// <param name="parser">The binary parser to read Currency</param>
        /// <returns>A Blob object</returns>
        public new static Currency FromParser(BinaryParser parser, int? hint = null)
        {
            return new Currency(parser.Read(20));
        }

        //public static UnissuedAmount operator /(decimal v, Currency c)
        //{
        //    if (c == Xrp)
        //    {
        //        v *= 1e6m;
        //    }
        //    return new UnissuedAmount(v, c);
        //}

        public static Issue operator /(Currency c, AccountId ac)
        {
            return new Issue();        //todo ?
        }
    }

    public class Issue
    {
        //todo ?
    }
}