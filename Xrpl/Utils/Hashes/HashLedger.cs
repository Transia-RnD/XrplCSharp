using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xrpl.BinaryCodec;
using Xrpl.BinaryCodec.Hashing;
using Xrpl.BinaryCodec.ShaMapTree;
using Xrpl.BinaryCodec.Util;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Ledger;
using Xrpl.Models.Transactions;
using Xrpl.Utils.Hashes.ShaMap;

using static Xrpl.AddressCodec.Utils;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/hashes/hashLedger.ts

namespace Xrpl.Utils.Hashes
{
    public interface HashLedgerHeaderOptions
    {
        public bool? ComputeTreeHashes { get; set; }

    }

    public class HashLedger
    {
        public static string HashSignedTx(string tx)
        {
            string txBlob = tx;
            Dictionary<string, dynamic> txObject = XrplBinaryCodec.Decode(tx).ToObject<Dictionary<string, dynamic>>();
            if (!txObject.ContainsKey("TxnSignature") && !txObject.ContainsKey("Signers"))
            {
                new ValidationException("The transaction must be signed to hash it.");
            }

            return B16.Encode(Sha512.Half(input: txBlob.FromHexToBytes(), prefix: (uint)Xrpl.BinaryCodec.Hashing.HashPrefix.TransactionId));
        }

        public static string HashSignedTx(JToken tx)
        {
            string txBlob = XrplBinaryCodec.Encode(tx);
            Dictionary<string, dynamic> txObject = tx.ToObject<Dictionary<string, dynamic>>();
            if (!txObject.ContainsKey("TxnSignature") && !txObject.ContainsKey("Signers"))
            {
                new ValidationException("The transaction must be signed to hash it.");
            }

            return B16.Encode(Sha512.Half(input: txBlob.FromHexToBytes(), prefix: (uint)Xrpl.BinaryCodec.Hashing.HashPrefix.TransactionId));
        }
    }
}

