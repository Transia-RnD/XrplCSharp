

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/verifyPaymentChannelClaim.ts

//todo DO

using Xrpl.Keypairs;
using Xrpl.Models.Transactions;

namespace Xrpl.Utils
{
    public static class VerifyPmntChannelClaim
    {

        /// <summary>
        /// Verify the signature of a payment channel claim.
        /// </summary>
        /// <param name="channel">Channel identifier specified by the paymentChannelClaim.</param>
        /// <param name="amount">Amount specified by the paymentChannelClaim.</param>
        /// <param name="signature">Signature produced from signing paymentChannelClaim.</param>
        /// <param name="publicKey">Public key that signed the paymentChannelClaim.</param>
        /// <returns>True if the channel is valid.</returns>
        /// <category>Utilities</category>
        public static bool VerifyPaymentChannelClaim(string channel, string amount, string signature, string publicKey)
        {
            var payment = new PaymentChannelClaim
            {
                Channel = channel,
                Amount = XrpConversion.XrpToDrops(amount)
            };

            var signingData = payment.EncodeForSigningClaim();
            return XrplKeypairs.Verify(signingData, signature, publicKey);
        }
    }
}

