using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec;
using Ripple.Binary.Codec.Hashing;
using Ripple.Binary.Codec.Types;
using Ripple.Binary.Codec.Util;
using Xrpl.Client.Exceptions;

namespace Xrpl.Utils.Hashes
{
    public class HashLedger
    {
        public static string HashSignedTx(string tx)
        {
            string txBlob = tx;
            Dictionary<string, dynamic> txObject = BinaryCodec.Decode(tx).ToObject<Dictionary<string, dynamic>>();
            if (!txObject.ContainsKey("TxnSignature") && !txObject.ContainsKey("Signers"))
            {
                new ValidationError("The transaction must be signed to hash it.");
            }
            return B16.Encode(Sha512.Half(input: Ripple.Address.Codec.Utils.FromHexToBytes(txBlob), prefix: (uint)HashPrefix.TransactionId));
        }

        public static string HashSignedTx(JToken tx)
        {
            string txBlob = BinaryCodec.Encode(tx);
            Dictionary<string, dynamic> txObject = tx.ToObject<Dictionary<string, dynamic>>();
            if (!txObject.ContainsKey("TxnSignature") && !txObject.ContainsKey("Signers"))
            {
                new ValidationError("The transaction must be signed to hash it.");
            }
            return B16.Encode(Sha512.Half(input: Ripple.Address.Codec.Utils.FromHexToBytes(txBlob), prefix: (uint)HashPrefix.TransactionId));
        }
    }
}

