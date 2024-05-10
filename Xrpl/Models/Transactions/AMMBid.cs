#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xrpl.Client.Exceptions;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;

using static Xrpl.Models.Common.Common;

// https://github.com/XRPLF/xrpl.js/blob/amm/packages/xrpl/src/models/transactions/AMMBid.ts

namespace Xrpl.Models.Transactions
{

    /// <summary>
    /// AMMBid is used for submitting a vote for the trading fee of an AMM Instance.
    /// Any XRPL account that holds LPToken for an AMM instance may submit this
    /// transaction to vote for the trading fee for that instance.
    /// </summary>
    public class AMMBid : TransactionCommon, IAMMBid
    {
        public AMMBid()
        {
            TransactionType = TransactionType.AMMBid;
        }

        /// <inheritdoc />
        [JsonConverter(typeof(IssuedCurrencyConverter))]
        public IssuedCurrency Asset { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(IssuedCurrencyConverter))]
        public IssuedCurrency Asset2 { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Xrpl.Models.Common.Currency? BidMin { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Xrpl.Models.Common.Currency? BidMax { get; set; }
        /// <inheritdoc />
        public List<AuthAccount> AuthAccounts { get; set; }
    }
    /// <summary>
    /// AMMBid is used for submitting a vote for the trading fee of an AMM Instance.
    /// Any XRPL account that holds LPToken for an AMM instance may submit this
    /// transaction to vote for the trading fee for that instance.
    /// </summary>
    public interface IAMMBid : ITransactionCommon
    {
        /// <summary>
        /// Specifies one of the pool assets (XRP or token) of the AMM instance.
        /// </summary>
        public IssuedCurrency Asset { get; set; }
        /// <summary>
        /// Specifies the other pool asset of the AMM instance.
        /// </summary>
        public IssuedCurrency Asset2 { get; set; }
        /// <summary>
        /// This field represents the minimum price that the bidder wants to pay for the slot.
        /// It is specified in units of LPToken.If specified let BidMin be X and let
        /// the slot-price computed by price scheduling algorithm be Y, then bidder always pays
        /// the max(X, Y).
        /// </summary>
        public Xrpl.Models.Common.Currency? BidMin { get; set; }
        /// <summary>
        /// This field represents the maximum price that the bidder wants to pay for the slot.
        /// It is specified in units of LPToken.
        /// </summary>
        public Xrpl.Models.Common.Currency? BidMax { get; set; }
        /// <summary>
        /// This field represents an array of XRPL account IDs that are authorized to trade
        /// at the discounted fee against the AMM instance.
        /// A maximum of four accounts can be provided.
        /// </summary>
        public List<AuthAccount> AuthAccounts { get; set; }
    }

    /// <inheritdoc cref="IAMMBid" />
    public class AMMBidResponse : TransactionResponseCommon, IAMMBid
    {
        #region Implementation of IAMMBid

        /// <inheritdoc />
        [JsonConverter(typeof(IssuedCurrencyConverter))]
        public IssuedCurrency Asset { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(IssuedCurrencyConverter))]
        public IssuedCurrency Asset2 { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency? BidMin { get; set; }
        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency? BidMax { get; set; }
        /// <inheritdoc />
        public List<AuthAccount> AuthAccounts { get; set; }

        #endregion
    }
    public partial class Validation
    {
        private const int MAX_AUTH_ACCOUNTS = 4;
        /// <summary>
        /// Verify the form and type of an AMMBid at runtime.
        /// </summary>
        /// <param name="tx">An AMMBid Transaction.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">When the AMMBid is Malformed.</exception>
        public static async Task ValidateAMMBid(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);

            if (!tx.TryGetValue("Asset", out var Asset1) || Asset1 is null)
            {
                throw new ValidationException("AMMBid: missing field Asset");
            }

            if (!Xrpl.Models.Transactions.Common.IsIssue(Asset1))
            {
                throw new ValidationException("AMMBid: Asset must be an Issue");
            }

            if (!tx.TryGetValue("Asset2", out var Asset2) || Asset2 is null)
            {
                throw new ValidationException("AMMBid: missing field Asset2");
            }

            if (!Xrpl.Models.Transactions.Common.IsIssue(Asset2))
            {
                throw new ValidationException("AMMBid: Asset2 must be an Issue");
            }

            if (tx.TryGetValue("BidMin", out var BidMin) && BidMin is not null && !Common.IsAmount(BidMin))
            {
                throw new ValidationException("AMMBid: BidMin must be an Amount");
            }

            if (tx.TryGetValue("BidMax", out var BidMax) && BidMax is not null && !Common.IsAmount(BidMax))
            {
                throw new ValidationException("AMMBid: BidMax must be an Amount");
            }

            if (tx.TryGetValue("AuthAccounts", out var AuthAccounts) && AuthAccounts is not null)
            {
                if (AuthAccounts is not List<Dictionary<string, dynamic>> auth_accounts)
                {
                    throw new ValidationException("AMMBid: AuthAccounts must be an AuthAccount array");
                }
                if (auth_accounts.Count > MAX_AUTH_ACCOUNTS)
                {
                    throw new ValidationException($"AMMBid: AuthAccounts length must not be greater than {MAX_AUTH_ACCOUNTS}");
                }

                ValidateAuthAccounts(tx["Account"], auth_accounts);
            }
        }

        public static bool ValidateAuthAccounts(string senderAddress, List<Dictionary<string, dynamic>> authAccounts)
        {
            foreach (var account in authAccounts)
            {
                if (!account.TryGetValue("AuthAccount", out var auth) || auth is not Dictionary<string, dynamic> { } auth_acc)
                    throw new ValidationException("AMMBid: invalid AuthAccounts");

                if (!auth_acc.TryGetValue("Account", out var acc) || acc is null)
                    throw new ValidationException("AMMBid: invalid AuthAccounts");
                if (acc is not string { })
                    throw new ValidationException("AMMBid: invalid AuthAccounts");
                if (acc is string { } s && s == senderAddress)
                    throw new ValidationException("AMMBid: AuthAccounts must not include sender's address");
            }

            return true;
        }
    }
}

