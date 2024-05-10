using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.Client.Exceptions;
using Xrpl.Models.Utils;

namespace Xrpl.Models.Transactions
{
    public static partial class Validation
    {
        /// <summary>
        /// Verify the form and type of a TrustSet at runtime.
        /// </summary>
        /// <param name="tx"> A TrustSet Transaction.</param>
        /// <exception cref="ValidationException">When the TrustSet is malformed.</exception>
        public static async Task Validate(Dictionary<string, dynamic> tx)
        {
            tx.TryGetValue("TransactionType", out var type);

            if(type is null)
                throw new ValidationException("Object does not have a `TransactionType`");
            if(type is not string)
                throw new ValidationException("Object's `TransactionType` is not a string");

            //var value = JsonConvert.DeserializeObject<TransactionCommon>(tx as dynamic);

            // eslint-disable-next-line @typescript-eslint/consistent-type-assertions -- okay here
            Flags.SetTransactionFlagsToNumber(tx);
            
            switch (type)
            {
                case "AccountDelete":
                    await ValidateAccountDelete(tx);
                    break;

                case "AccountSet":
                    await ValidateAccountSet(tx);
                    break;

                case "CheckCancel":
                    await ValidateCheckCancel(tx);
                    break;

                case "CheckCash":
                    await ValidateCheckCash(tx);
                    break;

                case "CheckCreate":
                    await ValidateCheckCreate(tx);
                    break;

                case "DepositPreauth":
                    await ValidateDepositPreauth(tx);
                    break;

                case "EscrowCancel":
                    await ValidateEscrowCancel(tx);
                    break;

                case "EscrowCreate":
                    await ValidateEscrowCreate(tx);
                    break;

                case "EscrowFinish":
                    await ValidateEscrowFinish(tx);
                    break;

                case "NFTokenAcceptOffer":
                    await ValidateNFTokenAcceptOffer(tx);
                    break;

                case "NFTokenBurn":
                    await ValidateNFTokenBurn(tx);
                    break;

                case "NFTokenCancelOffer":
                    await ValidateNFTokenCancelOffer(tx);
                    break;

                case "NFTokenCreateOffer":
                    await ValidateNFTokenCreateOffer(tx);
                    break;

                case "NFTokenMint":
                    await ValidateNFTokenMint(tx);
                    break;

                case "OfferCancel":
                    await ValidateOfferCancel(tx);
                    break;

                case "OfferCreate":
                    await ValidateOfferCreate(tx);
                    break;

                case "Payment":
                    await ValidatePayment(tx);
                    break;

                case "PaymentChannelClaim":
                    await ValidatePaymentChannelClaim(tx);
                    break;

                case "PaymentChannelCreate":
                    await ValidatePaymentChannelCreate(tx);
                    break;

                case "PaymentChannelFund":
                    await ValidatePaymentChannelFund(tx);
                    break;

                case "SetRegularKey":
                    await ValidateSetRegularKey(tx);
                    break;

                case "SignerListSet":
                    await ValidateSignerListSet(tx);
                    break;

                case "TicketCreate":
                    await ValidateTicketCreate(tx);
                    break;

                case "TrustSet":
                    await ValidateTrustSet(tx);
                    break;
                case "AMMBid":
                    await ValidateAMMBid(tx);
                    break;
                case "AMMDeposit":
                    await ValidateAMMDeposit(tx);
                    break;
                case "AMMCreate":
                    await ValidateAMMCreate(tx);
                    break;
                case "AMMDelete":
                    await ValidateAMMDelete(tx);
                    break;
                case "AMMVote":
                    await ValidateAMMVote(tx);
                    break;
                case "AMMWithdraw":
                    await ValidateAMMWithdraw(tx);
                    break;
                default:
                    throw new ValidationException($"Invalid field TransactionType: {type}");
            }
        }
    }
}
