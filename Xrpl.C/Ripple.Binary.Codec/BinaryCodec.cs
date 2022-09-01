using System;
using System.Diagnostics;
using Ripple.Address.Codec.Exceptions;
using System.Text;
using Org.BouncyCastle.Utilities.Encoders;
using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Types;
using Newtonsoft.Json.Linq;
using System.Transactions;
using System.Collections.Generic;
using Ripple.Binary.Codec.Hashing;
using System.Collections;
using System.Reflection.Emit;


// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/index.ts

namespace Ripple.Binary.Codec
{
    public class BinaryCodec
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binary"></param>
        /// <returns>JToken</returns>
        public static JToken Decode(string binary)
        {
            Debug.WriteLine("DECODING");
            var stobject = StObject.FromHex(binary);
            return stobject.ToJson();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns>string</returns>
        public static string Encode(JToken token)
        {
            Debug.WriteLine("ENCODING");
            Debug.WriteLine(token.ToString());
            return SerializeJson(token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns>string</returns>
        public static string Encode(object json)
        {
            JToken token = JToken.FromObject(json);
            return Encode(token);
        }

        /// <summary>
        /// Encode a transaction into binary format in preparation for signing. (Only encodes fields that are intended to be signed.)
        /// </summary>
        /// <param name="json"></param>
        /// <returns>string</returns>
        public static string EncodeForSigning(Dictionary<string, dynamic> json)
        {
            JToken token = JToken.FromObject(json);
            return SerializeJson(token, HashPrefix.TxSign.Bytes(), null, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns>string</returns>
        public static string SerializeJson(JToken json, byte[]? prefix = null, byte[]? suffix = null, bool signingOnly = false)
        {
            var list = new BytesList();
            if (prefix != null)
            {
                list.Put(prefix);
            }

            StObject so = StObject.FromJson(json, strict: true);
            list.Put(so.ToBytes());

            if (suffix != null)
            {
                list.Put(suffix);
            }
            return list.BytesHex();
        }
    }
}