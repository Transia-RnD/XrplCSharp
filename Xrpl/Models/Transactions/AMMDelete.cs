using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xrpl.Client.Exceptions;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;

using static Xrpl.Models.Common.Common;

namespace Xrpl.Models.Transactions
{
    /// <summary>
    /// Delete an empty Automated Market Maker (AMM) instance that could not be fully deleted automatically.
    /// Tip: The AMMWithdraw transaction automatically tries to delete an AMM, along with associated ledger
    /// entries such as empty trust lines, if it withdrew all the assets from the AMM's pool.
    /// However, if there are too many trust lines to the AMM account to remove in one transaction,
    /// it may stop before fully removing the AMM.Similarly, an AMMDelete transaction removes up to
    /// a maximum number of trust lines; in extreme cases, it may take several AMMDelete transactions
    /// to fully delete the trust lines and the associated AMM.
    /// In all cases, the AMM ledger entry and AMM account are deleted by the last such transaction.
    /// </summary>
    public class AMMDelete : TransactionCommon, IAMMDelete
    {
        public AMMDelete()
        {
            TransactionType = TransactionType.AMMDelete;
        }

        /// <inheritdoc />
        [JsonConverter(typeof(IssuedCurrencyConverter))]
        public IssuedCurrency Asset { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(IssuedCurrencyConverter))]
        public IssuedCurrency Asset2 { get; set; }
    }
    /// <summary>
    /// Delete an empty Automated Market Maker (AMM) instance that could not be fully deleted automatically.
    /// Tip: The AMMWithdraw transaction automatically tries to delete an AMM, along with associated ledger
    /// entries such as empty trust lines, if it withdrew all the assets from the AMM's pool.
    /// However, if there are too many trust lines to the AMM account to remove in one transaction,
    /// it may stop before fully removing the AMM.Similarly, an AMMDelete transaction removes up to
    /// a maximum number of trust lines; in extreme cases, it may take several AMMDelete transactions
    /// to fully delete the trust lines and the associated AMM.
    /// In all cases, the AMM ledger entry and AMM account are deleted by the last such transaction.
    /// </summary>
    public interface IAMMDelete : ITransactionCommon
    {
        /// <summary>
        /// The definition for one of the assets in the AMM's pool.
        /// </summary>
        public IssuedCurrency Asset { get; set; }
        /// <summary>
        /// The definition for the other asset in the AMM's pool.
        /// </summary>
        public IssuedCurrency Asset2 { get; set; }
    }

    /// <inheritdoc cref="IAMMDelete" />
    public class AMMDeleteResponse : TransactionResponseCommon, IAMMDelete
    {
        #region Implementation of IAMMDelete

        /// <inheritdoc />
        [JsonConverter(typeof(IssuedCurrencyConverter))]
        public IssuedCurrency Asset { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(IssuedCurrencyConverter))]
        public IssuedCurrency Asset2 { get; set; }

        #endregion
    }

    public partial class Validation
    {
        /// <summary>
        /// Verify the form and type of an AMMDelete at runtime.
        /// </summary>
        /// <param name="tx">An AMMDelete Transaction.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"> When the AMMDelete is Malformed.</exception>
        public static async Task ValidateAMMDelete(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);

            if (!tx.TryGetValue("Asset", out var Asset) || Asset is null)
            {
                throw new ValidationException("AMMDelete: missing field Asset");
            }

            if (!Xrpl.Models.Transactions.Common.IsAmount(Asset))
            {
                throw new ValidationException("AMMDelete: Asset must be a Currency");
            }
            if (!tx.TryGetValue("Asset2", out var Asset2) || Asset2 is null)
            {
                throw new ValidationException("AMMDelete: missing field Asset2");
            }

            if (!Xrpl.Models.Transactions.Common.IsAmount(Asset2))
            {
                throw new ValidationException("AMMDelete: Asset2 must be a Currency");
            }
        }
    }
}
