using System;
using System.Text;
using System.Text.RegularExpressions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/account-id.ts

namespace Xrpl.BinaryCodec.Types
{
    public class AccountId : Hash160
    {
        public static readonly AccountId DefaultAccountId = new AccountId(new byte[20]);

        public AccountId(byte[] bytes) : base(bytes)
        {
        }

        public static AccountId From(object value)
        {
            if (value is AccountId)
            {
                return (AccountId) value;
            }

            if (value is string)
            {
                if (string.IsNullOrEmpty((string) value))
                {
                    return new AccountId();
                }

                if (Regex.IsMatch((string) value, "^[A-F0-9]{40}$"))
                {
                    return new AccountId(Encoding.UTF8.GetBytes((string) value));
                }

                return FromBase58((string) value);
            }

            throw new Exception("Cannot construct AccountID from value given");
        }

        public static AccountId FromBase58(string value)
        {
            if (IsValidXAddress(value))
            {
                var classic = XAddressToClassicAddress(value);
                if (classic.Tag != false)
                {
                    throw new Exception("Only allowed to have tag on Account or Destination");
                }

                value = classic.ClassicAddress;
            }

            return new AccountId(Encoding.UTF8.GetBytes(DecodeAccountId(value)));
        }

        public string ToJson()
        {
            return ToBase58();
        }

        public string ToBase58()
        {
            return EncodeAccountId(Bytes);
        }
    }
}