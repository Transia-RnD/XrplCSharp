using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.Json.Nodes;
using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Util;

// https://github.com/XRPLF/xrpl.js/blob/amm/packages/ripple-binary-codec/src/types/issue.ts

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Hash with a width of 160 bits
    /// </summary>
    public class Issue: ISerializedType
    {
        public readonly byte[] _Bytes;

        public class IssueObject
        {
            public string Currency { get; set; }
            public string Issuer { get; set; }
        }

        /// <summary>
        /// Type guard for AmountObject
        /// </summary>
        public static bool IsIssueObject(JObject arg)
        {
            var keys = arg.Properties().Select(p => p.Name).ToList();
            keys.Sort();
            if (keys.Count == 1)
            {
                return keys[0] == "currency";
            }
            return keys.Count == 2 && keys[0] == "currency" && keys[1] == "issuer";
        }

        public static readonly Issue ZERO_ISSUED_CURRENCY = new Issue(new byte[20]);

        private Issue(byte[] buffer)
        {
            this._Bytes = buffer;
        }

        public static implicit operator Issue(byte[] buffer)
        {
            Contract.Assert(buffer.Length == 20, "buffer should be 20 bytes");
            return new Issue(buffer ?? ZERO_ISSUED_CURRENCY._Bytes);
        }

        /// <summary>
        /// Read an amount from a BinaryParser
        /// </summary>
        /// <param name="parser">BinaryParser to read the Amount from</param>
        /// <returns>An Amount object</returns>
        public static Issue FromParser(BinaryParser parser)
        {
            var currency = parser.Read(20);
            if (new Currency(currency).ToString() == "XRP")
            {
                return new Issue(currency);
            }
            var currencyAndIssuer = new byte[40];
            Buffer.BlockCopy(currency, 0, currencyAndIssuer, 0, 20);
            Buffer.BlockCopy(parser.Read(20), 0, currencyAndIssuer, 20, 20);
            return new Issue(currencyAndIssuer);
        }

        /// <summary>
        /// Get the JSON representation of this Amount
        /// </summary>
        /// <returns>the JSON interpretation of this.bytes</returns>
        public IssueObject ToJson()
        {
            var parser = new BufferParser(this.ToString());
            var currency = Currency.FromParser(parser) as Currency;

            if (currency.ToString() == "XRP")
            {
                return new IssueObject { Currency = currency.ToString() };
            }

            var issuer = AccountId.FromParser(parser) as AccountId;

            return new IssueObject
            {
                Currency = currency.ToString(),
                Issuer = issuer.ToString()
            };
        }

        public void ToBytes(IBytesSink sink)
        {
            throw new NotImplementedException();
        }

        JToken ISerializedType.ToJson()
        {
            throw new NotImplementedException();
        }
    }
}