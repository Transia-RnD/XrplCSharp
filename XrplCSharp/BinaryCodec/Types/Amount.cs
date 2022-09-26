using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using Xrpl.BinaryCodecLib.Binary;
using Xrpl.BinaryCodecLib.Types;
using Xrpl.BinaryCodecLib.Util;
using Xrpl.Models.Common;
using Xrpl.Models.Transactions;
using Xrpl.KeypairsLib;
using Xrpl.Models.Methods;
using static Xrpl.AddressCodecLib.B58;
using System.Numerics;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/amount.ts

namespace Xrpl.BinaryCodecLib.Types
{
    /// <summary>
    /// An amount of XRP or tokens. The length of the field is 64 bits for XRP or 384 bits (64+160+160) for tokens.
    /// </summary>
    public class Amount : ISerializedType
    {
        public const int MinExponent = -96;
        public const int MaxExponent = 80;
        public const int MaxIouPrecision = 16;
        static readonly decimal MaxDrops = decimal.Parse("1e17", NumberStyles.AllowExponent);
        static readonly decimal MinXrp = decimal.Parse("1e-6", NumberStyles.AllowExponent);

        public readonly byte[] Buffer;
        private Amount(byte[] decode)
        {
            this.Buffer = decode;
        }

        public static implicit operator Amount(byte[] value)
        {
            Debug.WriteLine(value);
            return new Amount(value);
        }

        /// <summary>
        /// Returns True if the given string contains a decimal point character.
        /// </summary>
        /// <param name="str">amount</param>
        /// <returns></returns>
        public static bool ContainsDecimal(string str)
        {
            return !str.Contains(".");
        }

        public static void VerifyXrpValue(string xrpValue)
        {
            // Contains no decimal point
            if (!ContainsDecimal(xrpValue))
            {
                throw new BinaryCodecException($"{xrpValue} is an invalid XRP amount.");
            }
            // Within valid range
            decimal dec = decimal.Parse(xrpValue, NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
            if (dec.Equals(0))
            {
                return;
            }
            
            if (Decimal.Compare(dec, (decimal)MinXrp) == -1 || Decimal.Compare(dec, (decimal)MaxDrops) == 1)
            {
                throw new BinaryCodecException($"{xrpValue} is an invalid XRP amount.");
            }
        }

        /// <summary>
        /// Validates the format of an issued currency amount value.
        /// Raises if value is invalid.
        /// </summary>
        /// <param name="icv">A string representing the "value" field of an issued currency amount.</param>
        /// <returns>None, but raises if issued_currency_value is not valid.</returns>
        /// <throws>XRPLBinaryCodecException: If issued_currency_value is invalid.</throws>
        public static void VerifyIouValue(string icv)
        {
            // Within valid range
            decimal dec = decimal.Parse(icv, NumberStyles.Float, CultureInfo.InvariantCulture);
            if (dec.Equals(0))
            {
                return;
            }
            int exponent = icv.Exponent();
            if (
                icv.Precision() > MaxIouPrecision ||
                exponent > MaxExponent ||
                exponent < MinExponent
            ) {
                throw new BinaryCodecException("Decimal precision out of range for issued currency value.");
            }
        }

        /// <summary>
        /// Serializes an XRP amount.
        /// </summary>
        /// <param name="value">A string representing a quantity of XRP.</param>
        /// <returns>The bytes representing the serialized XRP amount.</returns>
        public static byte[] SerializeXrpAmount(string value)
        {
            byte[] amount = new byte[8];
            Amount.VerifyXrpValue(value);
            var drops = BigInteger.Parse(value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowExponent);
            //int drops = int.Parse(value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowExponent);
            Debug.WriteLine(drops);
            drops |= (byte)0x40;
            return new byte[8];
        }


        //public class AmountObject
        //{
        //    public string Currency { get; set; }
        //    public string Issuer { get; set; }
        //    public string Value { get; set; }

        //    public AmountObject(string value, string currency = null, string issuer = null)
        //    {
        //        Currency = currency;
        //        Issuer = issuer;
        //        Value = value;
        //    }
        //}

        //private static bool IsAmountObject(JToken token)
        //{
        //    return (
        //        // TODO: Sort and count key values keys.length == 0
        //        token["currency"].Type == JTokenType.String &&
        //        token["issuer"].Type == JTokenType.String &&
        //        token["value"].Type == JTokenType.String
        //    );
        //}

        //public readonly byte[] Buffer;
        //private Amount(byte[] decode)
        //{
        //    this.Buffer = decode;
        //}

        /// <summary>
        /// Test if this amount is in units of Native Currency(XRP)
        /// </summary>
        /// <returns></returns>
        //public bool IsNative() => (this.Value.ToBytes()[0] & 0x80) == 0;

        /// <summary> Currency issuer </summary>
        public readonly AccountId Issuer;
        /// <summary> currency code </summary>
        public readonly Currency Currency;
        //public bool IsNative => Currency.IsNative;
        /// <summary> amount of currency </summary>
        //public AmountValue Value;
        /// <summary> Maximum Precision </summary>
        public const int MaximumIouPrecision = 16;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="value">amount</param>
        /// <param name="currency">currency code</param>
        /// <param name="issuer">issuer</param>
        //public Amount(AmountValue value, Currency currency=null, AccountId issuer=null)
        //{
        //    Debug.WriteLine("INIT value");
        //    Currency = currency ?? Currency.Xrp;
        //    Issuer = issuer ?? (Currency.IsNative ? AccountId.Zero : AccountId.Neutral);
        //    Value = value;
        //}
        ///// <summary>
        ///// constructor
        ///// </summary>
        ///// <param name="value">amount</param>
        ///// <param name="currency">currency code</param>
        ///// <param name="issuer">issuer</param>
        //public Amount(string value = "0", Currency currency = null, AccountId issuer = null) :
        //              this(AmountValue.FromString(value, currency == null || currency.IsNative), currency, issuer)
        //{
        //    //Debug.WriteLine(c.IsNative);
        //    Debug.WriteLine("INIT string");
        //}
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="value">amount</param>
        /// <param name="currency">currency code</param>
        /// <param name="issuer">issuer</param>
        //public Amount(decimal value, Currency currency, AccountId issuer=null) :
        //    this(value.ToString(CultureInfo.InvariantCulture), currency, issuer)
        //{
        //    Debug.WriteLine("INIT decimal");
        //}

        /// <inheritdoc />
        public void ToBytes(IBytesSink sink)
        {
            Debug.WriteLine("ToBytes");
            sink.Put(this.Buffer);
            //if (!IsNative())
            //{
            //    Currency.ToBytes(sink);
            //    Issuer.ToBytes(sink);
            //}
        }

        /// <summary> this object to string data </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Buffer.ToHex();
        }

        /// <inheritdoc />
        public JToken ToJson()
        {
            Debug.WriteLine("Amount TO JSON");
            //if (this.IsNative())
            //{
            //    Debug.WriteLine("Amount STRING");
            //    return Buffer.ToString();
            //}
            //Debug.WriteLine("Amount OBJECT");
            //return new JObject
            //{
            //    ["value"] = Buffer.ToString(),
            //    ["currency"] = Currency,
            //    ["issuer"] = Issuer,
            //};
            return "";
        }

        /// <summary> create instance from binary parser</summary>
        /// <param name="value">value</param>
        public static Amount FromValue(Amount value)
        {
            return value;
        }

        /// <summary> create instance from binary parser</summary>
        /// <param name="value">value</param>
        public static Amount FromValue(string value)
        {
            Debug.WriteLine("FromValue");
            byte[] values = Amount.SerializeXrpAmount(value);
            return new Amount(values);
        }

        /// <summary>
        /// Get amount from json representation
        /// </summary>
        /// <param name="token">json representation</param>
        /// <returns></returns>
        /// <exception cref="InvalidJsonException"></exception>
        public static Amount FromJson(JToken token)
        {
            return new Amount(new byte[8]);
            //if (value instanceof Amount) {
            //    return value
            //}

            //if (token.Type == JTokenType.String)
            //{
            //    // Set the top bit for IOU
            //    mantissa[0] |= 0x80;
            //    if (IsZero) return mantissa;

            //    if (notNegative)
            //    {
            //        mantissa[0] |= 0x40;
            //    }

            //    var exponent = Exponent;
            //    var exponentByte = 97 + exponent;
            //    mantissa[0] |= (byte)(exponentByte >> 2);
            //    mantissa[1] |= (byte)((exponentByte & 0x03) << 6);
            //    return new Amount((string)valueToken, (string)currencyToken, (string)issuerToken);
            //}
            Debug.WriteLine("Amount FromJson");
            Debug.WriteLine(token.Type);
            
            //switch (token.Type)
            //{
            //    case JTokenType.String:

            //        return new Amount(token.ToString());
            //    case JTokenType.Integer:
            //        return (ulong)token;
            //    case JTokenType.Object:
            //        if ((string)token["currency"] == "XRP")
            //        {
            //            return new Amount(token["value"].ToString());
            //        }
            //        var valueToken = token["value"];
            //        var currencyToken = token["currency"];
            //        var issuerToken = token["issuer"];

            //        if (valueToken == null)
            //            throw new InvalidJsonException("Amount object must contain property `value`.");

            //        if (currencyToken == null)
            //            throw new InvalidJsonException("Amount object must contain property `currency`.");

            //        if (issuerToken == null)
            //            throw new InvalidJsonException("Amount object must contain property `issuer`.");

            //        if (token.Children().Count() > 3)
            //            throw new InvalidJsonException("Amount object has too many properties.");

            //        if(valueToken.Type != JTokenType.String)
            //            throw new InvalidJsonException("Property `value` must be string.");

            //        if (currencyToken.Type != JTokenType.String)
            //            throw new InvalidJsonException("Property `currency` must be string.");

            //        if (issuerToken.Type != JTokenType.String)
            //            throw new InvalidJsonException("Property `issuer` must be string.");

            //        return new Amount((string)valueToken, (string)currencyToken, (string)issuerToken);
            //    default:
            //        throw new InvalidJsonException("Can not create Amount from `{token}`");
            //}
        }

        //public static implicit operator Amount(ulong a)
        //{
        //    return new Amount(a.ToString("D"));
        //}

        //public static implicit operator Amount(string v)
        //{
        //    return new Amount(v);
        //}
        /// <summary>
        /// get amount from binary parser
        /// </summary>
        /// <param name="parser">binary parser</param>
        /// <param name="hint"></param>
        /// <returns></returns>
        public static Amount FromParser(BinaryParser parser, int? hint=null)
        {
            var value = AmountValue.FromParser(parser);
            //if (!value.IsIou) return new Amount(value);
            var curr = Currency.FromParser(parser);
            var issuer = AccountId.FromParser(parser);
            //return new Amount(value, curr, issuer);
            return new Amount(new byte[8]);
        }
        /// <summary>
        /// get decimal value of amount
        /// </summary>
        /// <returns></returns>
        public decimal DecimalValue()
        {
            return decimal.Parse(Buffer.ToString(), NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture);
        }

        //public static Amount operator * (Amount a, decimal b)
        //{
        //    return new Amount(
        //        (a.DecimalValue() * b).ToString(CultureInfo.InvariantCulture), 
        //                      a.Currency, a.Issuer);
        //}

        //public static bool operator < (decimal a, Amount b)
        //{
        //    return a < b.DecimalValue();
        //}

        //public static bool operator >(decimal a, Amount b)
        //{
        //    return a > b.DecimalValue();
        //}
        /// <summary>
        /// create amount value from decimal
        /// </summary>
        /// <param name="decimal"></param>
        /// <returns></returns>
        //public Amount NewValue(decimal @decimal)
        //{
        //    return new Amount(@decimal, Currency, Issuer);
        //}
    }
}