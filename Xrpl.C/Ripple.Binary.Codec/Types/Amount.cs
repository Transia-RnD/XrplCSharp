using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities;
using Ripple.Binary.Codec.Binary;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/amount.ts

namespace Ripple.Binary.Codec.Types
{
    public class Amount : ISerializedType
    {

        // V2 REFACTOR

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

        public bool IsNative() {
            return (this.Value.ToBytes()[0] & 0x80) == 0;
          }

        public readonly AccountId Issuer;
        public readonly Currency Currency;
        //public bool IsNative => Currency.IsNative;
        public AmountValue Value;

        public const int MaximumIouPrecision = 16;

        public Amount(AmountValue value, Currency currency=null, AccountId issuer=null)
        {
            Debug.WriteLine("INIT value");
            Currency = currency ?? Currency.Xrp;
            Issuer = issuer ?? (Currency.IsNative ? AccountId.Zero : AccountId.Neutral);
            Value = value;
        }

        public Amount(string v="0", Currency c=null, AccountId i=null) :
                      this(AmountValue.FromString(v, c == null || c.IsNative), c, i)
        {
            //Debug.WriteLine(c.IsNative);
            Debug.WriteLine("INIT string");
        }

        public Amount(decimal value, Currency currency, AccountId issuer=null) :
            this(value.ToString(CultureInfo.InvariantCulture), currency, issuer)
        {
            Debug.WriteLine("INIT decimal");
        }

        public void ToBytes(IBytesSink sink)
        {
            sink.Put(Value.ToBytes());
            if (!IsNative())
            {
                Currency.ToBytes(sink);
                Issuer.ToBytes(sink);
            }
        }

        public JToken ToJson()
        {
            Debug.WriteLine("Amount TO JSON");
            if (this.IsNative())
            {
                Debug.WriteLine("Amount STRING");
                return Value.ToString();
            }
            Debug.WriteLine("Amount OBJECT");
            return new JObject
            {
                ["value"] = Value.ToString(),
                ["currency"] = Currency,
                ["issuer"] = Issuer,
            };
        }

        public static Amount FromJson(JToken token)
        {
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
            
            switch (token.Type)
            {
                case JTokenType.String:

                    return new Amount(token.ToString());
                case JTokenType.Integer:
                    return (ulong)token;
                case JTokenType.Object:
                    if ((string)token["currency"] == "XRP")
                    {
                        return new Amount(token["value"].ToString());
                    }
                    var valueToken = token["value"];
                    var currencyToken = token["currency"];
                    var issuerToken = token["issuer"];

                    if (valueToken == null)
                        throw new InvalidJsonException("Amount object must contain property `value`.");

                    if (currencyToken == null)
                        throw new InvalidJsonException("Amount object must contain property `currency`.");

                    if (issuerToken == null)
                        throw new InvalidJsonException("Amount object must contain property `issuer`.");

                    if (token.Children().Count() > 3)
                        throw new InvalidJsonException("Amount object has too many properties.");

                    if(valueToken.Type != JTokenType.String)
                        throw new InvalidJsonException("Property `value` must be string.");

                    if (currencyToken.Type != JTokenType.String)
                        throw new InvalidJsonException("Property `currency` must be string.");

                    if (issuerToken.Type != JTokenType.String)
                        throw new InvalidJsonException("Property `issuer` must be string.");

                    return new Amount((string)valueToken, (string)currencyToken, (string)issuerToken);
                default:
                    throw new InvalidJsonException("Can not create Amount from `{token}`");
            }
        }

        public static implicit operator Amount(ulong a)
        {
            return new Amount(a.ToString("D"));
        }

        public static implicit operator Amount(string v)
        {
            return new Amount(v);
        }

        public static Amount FromParser(BinaryParser parser, int? hint=null)
        {
            var value = AmountValue.FromParser(parser);
            if (!value.IsIou) return new Amount(value);
            var curr = Currency.FromParser(parser);
            var issuer = AccountId.FromParser(parser);
            return new Amount(value, curr, issuer);
        }

        public decimal DecimalValue()
        {
            return decimal.Parse(Value.ToString(), NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture);
        }

        public static Amount operator * (Amount a, decimal b)
        {
            return new Amount(
                (a.DecimalValue() * b).ToString(CultureInfo.InvariantCulture), 
                              a.Currency, a.Issuer);
        }

        public static bool operator < (decimal a, Amount b)
        {
            return a < b.DecimalValue();
        }

        public static bool operator >(decimal a, Amount b)
        {
            return a > b.DecimalValue();
        }

        public Amount NewValue(decimal @decimal)
        {
            return new Amount(@decimal, Currency, Issuer);
        }
    }
}