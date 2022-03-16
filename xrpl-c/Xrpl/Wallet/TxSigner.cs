using Newtonsoft.Json.Linq;
using Ripple.Core.Enums;
using Ripple.Core.Transactions;
using Ripple.Core.Types;
using Ripple.Core.Util;
using Ripple.Signing;

// ReSharper disable RedundantArgumentNameForLiteralExpression

namespace Ripple.TxSigning
{
    public class TxSigner
    {
        public const uint CanonicalSigFlag = 0x80000000u;

        private readonly IKeyPair _keyPair;

        private TxSigner(IKeyPair keyPair)
        {
            _keyPair = keyPair;
        }
        private TxSigner(string secret) :
            this(Seed.FromBase58(secret).KeyPair())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tx"></param>
        /// <returns></returns>
        /// <exception cref="InvalidTxException">Thrown when provided Json transaction is not valid.</exception>
        public SignedTx SignJson(JObject tx)
        {
            StObject so;

            try
            {
                so = StObject.FromJson(tx, strict: true);
            } catch(InvalidJsonException ex)
            {
                throw new InvalidTxException("Transaction is not valid.", nameof(tx), ex);
            }
            return SignStObject(so);
        }

        public static TxSigner FromKeyPair(IKeyPair keyPair)
        {
            return new TxSigner(keyPair);
        }

        public static TxSigner FromSecret(string secret)
        {
            return new TxSigner(secret);
        }

        public static SignedTx SignJson(JObject tx, string secret)
        {
            return FromSecret(secret).SignJson(tx);
        }

        public SignedTx SignStObject(StObject tx)
        {
            tx.SetFlag(CanonicalSigFlag);
            tx[Field.SigningPubKey] = _keyPair.CanonicalPubBytes();
            tx[Field.TxnSignature] = _keyPair.Sign(tx.SigningData());
            return ValidateAndEncode(tx);
        }

        public static SignedTx ValidateAndEncode(StObject tx)
        {
            try
            {
                TxFormat.Validate(tx);
            }
            catch (TxFormatValidationException ex)
            {
                throw new InvalidTxException("Transaction is not valid.", nameof(tx), ex);
            }

            var blob = tx.ToBytes();
            var hash = Utils.TransactionId(blob);
            return new SignedTx(hash, B16.Encode(blob), tx.ToJsonObject());
        }
    }
}
