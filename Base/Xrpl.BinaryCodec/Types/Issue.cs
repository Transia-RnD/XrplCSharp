using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.Json.Nodes;
using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Util;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/issue.ts

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Hash with a width of 160 bits
    /// </summary>
    public class Issue: SerializedType
    {
        //public readonly byte[] Buffer;

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
            this.Buffer = buffer;
        }

        public static implicit operator Issue(byte[] buffer)
        {
            Contract.Assert(buffer.Length == 20, "buffer should be 20 bytes");
            return new Issue(buffer ?? ZERO_ISSUED_CURRENCY.Buffer);
        }


        /// <summary> create instance from json object </summary>
        /// <param name="token">json object</param>
        public static Issue FromJson(JToken token)
        {
            return new Issue(B16.Decode(token.ToString()));
        }
        /// <summary> create instance from binary parser</summary>
        /// <param name="parser">parser</param>
        /// <param name="hint"></param>
        public static Issue FromParser(BinaryParser parser, int? hint = null)
        {
            return new Issue(parser.Read(20));
        }
    }
}