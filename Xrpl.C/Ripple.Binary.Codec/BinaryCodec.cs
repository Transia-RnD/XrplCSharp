using System;
using System.Diagnostics;
using Ripple.Address.Codec.Exceptions;
using System.Text;
using Org.BouncyCastle.Utilities.Encoders;
using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Types;
using Newtonsoft.Json.Linq;
using System.Transactions;


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
            var stobject = StObject.FromJson(token);
            return Ripple.Address.Codec.Utils.FromBytesToHex(stobject.SigningData());
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
    }
}