using System;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec;
using Ripple.Binary.Codec.Hashing;
using Ripple.Binary.Codec.Util;
using System.Collections.Generic;
using Xrpl.Client.Exceptions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/hashes/index.ts

namespace Xrpl.Utils.Hashes
{
    public class Hashes
    {
        public static string HashPaymentChannel(string address, string dstAddress, int sequence)
        {
            return "";
            //string txBlob = BinaryCodec.Encode(tx);
            //Dictionary<string, dynamic> txObject = tx.ToObject<Dictionary<string, dynamic>>();
            //if (!txObject.ContainsKey("TxnSignature") && !txObject.ContainsKey("Signers"))
            //{
            //    new ValidationError("The transaction must be signed to hash it.");
            //}
            //return B16.Encode(Sha512.Half(input: Ripple.Address.Codec.Utils.FromHexToBytes(txBlob), prefix: (uint)HashPrefix.TransactionId));
        }
    }
}

