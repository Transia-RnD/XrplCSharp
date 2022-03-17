using Newtonsoft.Json.Linq;

namespace Xrpl.Wallet
{
    public class SignedTx
    {
        public readonly string Hash;
        public readonly string TxBlob;
        public readonly JObject TxJson;

        public SignedTx(string hash, string txBlob, JObject txJson)
        {
            Hash = hash;
            TxBlob = txBlob;
            TxJson = txJson;
        }
    }
}
