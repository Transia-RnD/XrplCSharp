using System;
using Newtonsoft.Json.Linq;
using Ripple.Address.Codec;
using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Util;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/account-id.ts

namespace Ripple.Binary.Codec.Types
{
    /// <summary>
    /// Class defining how to encode and decode an AccountID<br/>
    /// The unique identifier for an account.
    /// </summary>
    public class AccountId : Hash160
    {
        private string _encoded;

        private string Encoded
        {
            get => _encoded ??= XrplCodec.EncodeAccountID(Buffer);
            set => _encoded = value;
        }
        /// <summary>
        /// Defines how to construct an AccountID
        /// </summary>
        /// <param name="hash">value either an existing AccountID, a hex-string</param>
        /// <param name="encoded">hex encoded</param>
        public AccountId(byte[] hash, string encoded) : base(hash)
        {
            Encoded = encoded;
        }
        /// <summary>
        /// Defines how to construct an AccountID
        /// </summary>
        /// <param name="v">value either an existing AccountID, a base58 r-Address</param>
        public AccountId(string v) :
            this(XrplCodec.DecodeAccountID(v), v) {}
        /// <summary>
        /// Defines how to construct an AccountID
        /// </summary>
        /// <param name="hash">a hex-string</param>
        public AccountId(byte[] hash) :
            this(hash, XrplCodec.EncodeAccountID(hash)) {}
        /// <summary> create instance from byte object </summary>
        /// <param name="value">string object</param>
        public static implicit operator AccountId(string value)
        {
            return new AccountId(value);
        }
        /// <summary> create instance from byte object </summary>
        /// <param name="value">object</param>
        public static implicit operator AccountId(uint value)
        {
            byte[] empty = new byte[20];
            var sourceArray = Bits.GetBytes(value);
            Array.Copy(sourceArray, 0, empty, 16, 4);
            return new AccountId(empty);
        }
        /// <summary> create instance from json object </summary>
        /// <param name="json">json object</param>
        public static implicit operator AccountId(JToken json)
        {
            return json == null ? null : FromJson(json);
        }
        /// <summary> create json object from this element </summary>
        /// <param name="v">AccountId object</param>
        public static implicit operator JToken(AccountId v)
        {
            return v.ToString();
        }
        /// <summary> this object to string data </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Encoded;
        }
        /// <summary> create instance from json object </summary>
        /// <param name="json">json object</param>
        public new static AccountId FromJson(JToken json)
        {
            return json?.ToString();
        }

        public static readonly AccountId Zero = 0;
        public static readonly AccountId Neutral = 1;

        /// <summary> create instance from binary parser</summary>
        /// <param name="parser">parser</param>
        /// <param name="hint"></param>
        public new static AccountId FromParser(BinaryParser parser, int? hint=null)
        {
            return new AccountId(parser.Read(20));
        }
    }
}