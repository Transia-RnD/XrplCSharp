using System.Diagnostics;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/hashes/index.ts

namespace Xrpl.Utils.Hashes
{
    public class Hashes
    {
        public static string HashPaymentChannel(string address, string dstAddress, int sequence)
        {
            Debug.WriteLine("FAILING HERE");
            return "";
            //string txBlob = BinaryCodec.Encode(tx);
            //Dictionary<string, dynamic> txObject = tx.ToObject<Dictionary<string, dynamic>>();
            //if (!txObject.ContainsKey("TxnSignature") && !txObject.ContainsKey("Signers"))
            //{
            //    new ValidationException("The transaction must be signed to hash it.");
            //}
            //return B16.Encode(Sha512.Half(input: Address.Codec.Utils.FromHexToBytes(txBlob), prefix: (uint)HashPrefix.TransactionId));
        }
    }
}

