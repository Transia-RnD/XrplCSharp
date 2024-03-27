

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/signPaymentChannelClaim.ts

//todo DO
using Xrpl.Keypairs;
using Xrpl.Models.Transactions;

namespace Xrpl.Utils
{
    public static class SignPmntChannelClaim
    {

        /// <summary>
        /// Sign a payment channel claim.
        /// </summary>
        /// <param name="channel"> Channel identifier specified by the paymentChannelClaim.</param>
        /// <param name="amount">Amount specified by the paymentChannelClaim.</param>
        /// <param name="privateKey">Private Key to sign paymentChannelClaim with.</param>
        /// <returns>True if the channel is valid.</returns>
        public static string SignPaymentChannelClaim(string channel, string amount, string privateKey)
        {

            var payment = new PaymentChannelClaim
            {
                Channel = channel,
                Amount = XrpConversion.XrpToDrops(amount)
            };

            var signingData = payment.EncodeForSigningClaim();

            return XrplKeypairs.Sign(signingData, privateKey);
        }
    }
}

