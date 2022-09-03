using System;
using Newtonsoft.Json.Linq;
using Ripple.Address.Codec;
using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Util;

namespace Ripple.Binary.Codec.Types
{
    public class AccountId : Hash160
    {
        private string _encoded;
        private string Encoded
        {
            get
            {
                return _encoded ?? (
                    _encoded =
                    XrplCodec.EncodeAddress(Buffer));
            }
            set { _encoded = value; }
        }

        public AccountId(byte[] hash, string encoded) : base(hash)
        {
            Encoded = encoded;
        }

        public AccountId(string v) :
            this(XrplCodec.DecodeAddress(v), v) {}

        public AccountId(byte[] hash) :
            this(hash, XrplCodec.EncodeAddress(hash)) {}

        public static implicit operator AccountId(string value)
        {
            return new AccountId(value);
        }

        public static implicit operator AccountId(uint value)
        {
            byte[] empty = new byte[20];
            var sourceArray = Bits.GetBytes(value);
            Array.Copy(sourceArray, 0, empty, 16, 4);
            return new AccountId(empty);
        }
        public static implicit operator AccountId(JToken json)
        {
            return json == null ? null : FromJson(json);
        }

        public static implicit operator JToken(AccountId v)
        {
            return v.ToString();
        }

        public override string ToString()
        {
            return Encoded;
        }

        public new static AccountId FromJson(JToken json)
        {
            return json?.ToString();
        }

        public static readonly AccountId Zero = 0;
        public static readonly AccountId Neutral = 1;

        public new static AccountId FromParser(BinaryParser parser, int? hint=null)
        {
            return new AccountId(parser.Read(20));
        }
    }
}