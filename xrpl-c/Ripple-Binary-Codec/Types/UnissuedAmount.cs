using System.Globalization;

namespace Ripple.Core.Types
{
    public class UnissuedAmount
    {
        private readonly Currency _currency;
        private readonly decimal _value;

        public UnissuedAmount(decimal value, Currency currency)
        {
            _value = value;
            _currency = currency;
        }

        public static Amount operator / (UnissuedAmount ui, AccountId issuer)
        {
            return new Amount(ui._value, ui._currency, issuer);
        }

        public static implicit operator Amount(UnissuedAmount a)
        {
            return new Amount(a._value, a._currency);
        }
    }
}