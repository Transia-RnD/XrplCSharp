using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodecLib;
using Xrpl.BinaryCodecLib.Hashing;
using Xrpl.BinaryCodecLib.Util;
using Xrpl.Client.Exceptions;
using static Xrpl.AddressCodecLib.Utils;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/hashes/hashLedger.ts

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
            return B16.Encode(Sha512.Half(input: FromHexToBytes(txBlob), prefix: (uint)HashPrefix.TransactionId));
        }

        public static string HashSignedTx(JToken tx)
        {
            string txBlob = BinaryCodec.Encode(tx);
            Dictionary<string, dynamic> txObject = tx.ToObject<Dictionary<string, dynamic>>();
            if (!txObject.ContainsKey("TxnSignature") && !txObject.ContainsKey("Signers"))
            {
                new ValidationError("The transaction must be signed to hash it.");
            }
            return B16.Encode(Sha512.Half(input: FromHexToBytes(txBlob), prefix: (uint)HashPrefix.TransactionId));
        }
    }
}

