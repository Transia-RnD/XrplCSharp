using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xrpl.Client.Exceptions;
using static Xrpl.Models.Common.Common;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.BinaryCodec.Types;
using Currency = Xrpl.Models.Common.Currency;

namespace Xrpl.Models.Transactions
{
    /// <summary>
    /// The Clawback transaction is used by the token issuer to claw back issued tokens from a holder.
    /// </summary>
    public class ClawBack : TransactionCommon, IClawBack
    {
        public ClawBack()
        {
            TransactionType = TransactionType.Clawback;
        }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Xrpl.Models.Common.Currency Amount { get; set; }
    }
    /// <summary>
    /// ClawBack is used for submitting a vote for the trading fee of an AMM Instance.
    /// Any XRPL account that holds LPToken for an AMM instance may submit this
    /// transaction to vote for the trading fee for that instance.
    /// </summary>
    public interface IClawBack : ITransactionCommon
    {
        /// <summary>
        /// The amount of currency to deliver, and it must be non-XRP.
        /// The nested field names MUST be lower-case. The `issuer` field MUST be the holder's address, whom to be clawed back.
        /// </summary>
        public Xrpl.Models.Common.Currency Amount { get; set; }
    }

    /// <inheritdoc cref="IClawBack" />
    public class ClawBackResponse : TransactionResponseCommon, IClawBack
    {
        #region Implementation of IClawBack

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        #endregion
    }
    public partial class Validation
    {
        /// <summary>
        /// Verify the form and type of an ClawBack at runtime.
        /// </summary>
        /// <param name="tx">An ClawBack Transaction.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">When the ClawBack is Malformed.</exception>
        public static async Task ValidateClawBack(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);

            if (!tx.TryGetValue("Amount", out var Amount) || Amount is null)
            {
                throw new ValidationException("ClawBack: missing field Amount");
            }

            if (!Xrpl.Models.Transactions.Common.IsIssuedCurrency(Amount))
            {
                throw new ValidationException("ClawBack: invalid Amount");
            }

            if (!tx.TryGetValue("Account", out var acc) || acc is null)
                throw new ValidationException("ClawBack: invalid Account");
            var amount = JsonConvert.DeserializeObject<Currency>($"{Amount}");
            if (amount.Issuer == acc)
            {
                throw new ValidationException("ClawBack: invalid holder Account");
            }
        }
    }

}
