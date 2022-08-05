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
            get =>
                _encoded ??= AddressCodec.EncodeAddress(Buffer);
            set => _encoded = value;
        }

        public AccountId(byte[] hash, string encoded) : base(hash) => Encoded = encoded;

        public AccountId(string v) :
            this(AddressCodec.DecodeAddress(v), v) {}
        public AccountId(byte[] hash) :
            this(hash, AddressCodec.EncodeAddress(hash)) {}

        public static implicit operator AccountId(string value) => new(value);

        public static implicit operator AccountId(uint value)
        {
            var empty = new byte[20];
            var sourceArray = Bits.GetBytes(value);
            Array.Copy(sourceArray, 0, empty, 16, 4);
            return new AccountId(empty);
        }
        public static implicit operator AccountId(JToken json) => json == null ? null : FromJson(json);

        public static implicit operator JToken(AccountId v) => v.ToString();

        public override string ToString() => Encoded;

        public new static AccountId FromJson(JToken json) => json?.ToString();

        public static readonly AccountId Zero = 0;
        public static readonly AccountId Neutral = 1;

        public new static AccountId FromParser(BinaryParser parser, int? hint=null) => new(parser.Read(20));
    }
}