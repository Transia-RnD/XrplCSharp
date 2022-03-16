using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Ripple.Core.Binary;

namespace Ripple.Core.Types
{
    public class Amount : ISerializedType
    {
        public readonly AccountId Issuer;
        public readonly Currency Currency;
        public bool IsNative => Currency.IsNative;
        public AmountValue Value;

        public const int MaximumIouPrecision = 16;

        public Amount(AmountValue value,
                      Currency currency=null,
                      AccountId issuer=null)
        {
            Currency = currency ?? Currency.Xrp;
            Issuer = issuer ?? (Currency.IsNative ?
                                    AccountId.Zero :
                                    AccountId.Neutral);
            Value = value;
        }

        public Amount(string v="0", Currency c=null, AccountId i=null) :
                      this(AmountValue.FromString(v, c == null || c.IsNative), c, i)
        {
        }

        public Amount(decimal value, Currency currency, AccountId issuer=null) :
            this(value.ToString(CultureInfo.InvariantCulture), currency, issuer)
        {
        }

        public void ToBytes(IBytesSink sink)
        {
            sink.Put(Value.ToBytes());
            if (!IsNative)
            {
                Currency.ToBytes(sink);
                Issuer.ToBytes(sink);
            }
        }

        public JToken ToJson()
        {
            if (IsNative)
            {
                return Value.ToString();
            }
            return new JObject
            {
                ["value"] = Value.ToString(),
                ["currency"] = Currency,
                ["issuer"] = Issuer,
            };
        }

        public static Amount FromJson(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Integer:
                    return (ulong)token;
                case JTokenType.String:
                    return new Amount(token.ToString());
                case JTokenType.Object:
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
            return decimal.Parse(Value.ToString());
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