using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xrpl.Client.Exceptions;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;
using Xrpl.Models.Methods;
using Xrpl.Models.Utils;

using static System.Net.WebRequestMethods;
using Index = Xrpl.Models.Utils.Index;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/payment.ts

namespace Xrpl.Models.Transaction
{
    /// <summary>
    /// Enum representing values for Payment Transaction Flags.
    /// </summary>
    [Flags]
    public enum PaymentFlags : uint
    {
        /// <summary>
        /// Do not use the default path;<br/>
        /// only use paths included in the Paths field.<br/>
        /// This is intended to force the transaction to take arbitrage opportunities.<br/>
        /// Most clients do not need this.
        /// </summary>
        tfNoDirectRipple = 65536,
        /// <summary>
        /// If the specified Amount cannot be sent without spending more than SendMax, reduce the received amount instead of failing outright.<br/>
        /// See Partial Payments for more details.
        /// </summary>
        tfPartialPayment = 131072,
        /// <summary>
        /// Only take paths where all the conversions have an input:output ratio that is equal or better than the ratio of Amount:SendMax.<br/>
        /// See Limit Quality for details.
        /// </summary>
        tfLimitQuality = 262144,
    }

    /// <inheritdoc cref="IPayment" />
    public class Payment : TransactionCommon, IPayment
    {
        public Payment()
        {
            TransactionType = TransactionType.Payment;
        }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        /// <inheritdoc />
        public string Destination { get; set; }

        /// <inheritdoc />
        public uint? DestinationTag { get; set; }

        /// <inheritdoc />
        public new PaymentFlags? Flags { get; set; }

        /// <inheritdoc />
        public string InvoiceID { get; set; }

        /// <inheritdoc />
        public List<List<Path>> Paths { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency SendMax { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency DeliverMin { get; set; }
    }

    /// <summary>
    /// A Payment transaction represents a transfer of value from one account to  another.
    /// </summary>
    /// <code>
    /// ```typescript
    /// const partialPayment: Payment =
    /// {
    /// 	TransactionType: 'Payment',
    /// 	Account: 'rM9WCfJU6udpFkvKThRaFHDMsp7L8rpgN',
    /// 	Amount:{
    /// 	currency: 'FOO',
    /// 	value: '4000',
    /// 	issuer: 'rPzwM2JfCSDjhbesdTCqFjWWdK7eFtTwZz',
    ///     },
    /// 	Destination: 'rPzwM2JfCSDjhbesdTCqFjWWdK7eFtTwZz',
    /// 	Flags:{
    /// 	tfPartialPayment: true
    ///     }
    /// }
    /// ```
    /// </code>
    public interface IPayment : ITransactionCommon
    {
        /// <summary>
        /// The amount of currency to deliver.<br/>
        /// For non-XRP amounts, the nested field names MUST be lower-case.<br/>
        /// If the tfPartialPayment flag is set, deliver up to this amount instead.
        /// </summary>
        Currency Amount { get; set; }
        /// <summary>
        /// Minimum amount of destination currency this transaction should deliver.<br/>
        /// Only valid if this is a partial payment.<br/>
        /// For non-XRP amounts, the nested field names are lower-case.
        /// </summary>
        Currency DeliverMin { get; set; }
        /// <summary>
        /// The unique address of the account receiving the payment.
        /// </summary>
        string Destination { get; set; }
        /// <summary>
        /// Arbitrary tag that identifies the reason for the payment to the destination, or a hosted recipient to pay.
        /// </summary>
        uint? DestinationTag { get; set; }
        /// <summary>
        /// Payment Transaction Flags
        /// </summary>
        new PaymentFlags? Flags { get; set; }
        /// <summary>
        /// Arbitrary 256-bit hash representing a specific reason or identifier for this payment.
        /// </summary>
        string InvoiceID { get; set; }
        /// <summary>
        /// Array of payment paths to be used for this transaction.<br/>
        /// Must be omitted for XRP-to-XRP transactions.
        /// </summary>
        List<List<Path>> Paths { get; set; }
        /// <summary>
        /// Highest amount of source currency this transaction is allowed to cost, including transfer fees, exchange rates, and slippage.<br/>
        /// Does not include the XRP destroyed as a cost for submitting the transaction.<br/>
        /// For non-XRP amounts, the nested field names MUST be lower-case.<br/>
        /// Must be supplied for cross-currency/cross-issue payments.<br/>
        /// Must be omitted for XRP-to-XRP Payments.
        /// </summary>
        Currency SendMax { get; set; }
    }

    /// <inheritdoc cref="IPayment" />
    public class PaymentResponse : TransactionResponseCommon, IPayment
    {
        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency Amount { get; set; }

        /// <inheritdoc />
        public string Destination { get; set; }

        /// <inheritdoc />
        public uint? DestinationTag { get; set; }

        /// <inheritdoc />
        public new PaymentFlags? Flags { get; set; }

        /// <inheritdoc />
        public string InvoiceID { get; set; }

        /// <inheritdoc />
        public List<List<Path>> Paths { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency SendMax { get; set; }

        /// <inheritdoc />
        [JsonConverter(typeof(CurrencyConverter))]
        public Currency DeliverMin { get; set; }
    }

    public partial class Validation
    {
        /// <summary>
        /// Verify the form and type of a Payment at runtime.
        /// </summary>
        /// <param name="tx"> A Payment Transaction.</param>
        /// <exception cref="ValidationError">When the Payment is malformed.</exception>
        public static async Task ValidatePayment(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);

            if (!tx.TryGetValue("Amount", out var Amount) || Amount is null)
                throw new ValidationError("PaymentTransaction: missing field Amount");

            if (!Common.IsAmount(Amount))
                throw new ValidationError("PaymentTransaction: invalid Amount");


            if (!tx.TryGetValue("Destination", out var Destination) || Destination is null)
                throw new ValidationError("PaymentTransaction: missing field Destination");
            if (!Common.IsAmount(Destination))
                throw new ValidationError("PaymentTransaction: invalid Destination");

            if (tx.TryGetValue("DestinationTag", out var DestinationTag) && DestinationTag is not uint { })
                throw new ValidationError("PaymentTransaction: DestinationTag must be a number");

            if (tx.TryGetValue("InvoiceID", out var InvoiceID) && InvoiceID is not string { })
                throw new ValidationError("PaymentTransaction: InvoiceID must be a string");
            if (tx.TryGetValue("Paths", out var Paths) && !IsPaths(Paths as List<List<Dictionary<string, dynamic>>>))
                throw new ValidationError("PaymentTransaction: invalid Paths");
            if (tx.TryGetValue("SendMax", out var SendMax) && !Common.IsAmount(SendMax))
                throw new ValidationError("PaymentTransaction: invalid SendMax");

            await CheckPartialPayment(tx);
        }

        public static Task CheckPartialPayment(Dictionary<string, dynamic> tx)
        {
            tx.TryGetValue("Flags", out var flags);

            if (tx.TryGetValue("DeliverMin", out var DeliverMin) )
            {
                if (flags is null)
                    throw new ValidationError("PaymentTransaction: tfPartialPayment flag required with DeliverMin");
            }

            //todo check func
            var isTfPartialPayment = flags is uint { } flag
                ? Index.IsFlagEnabled(flag, (uint)PaymentFlags.tfPartialPayment)
                : flags is PaymentFlags && flags == PaymentFlags.tfPartialPayment;

            if (!isTfPartialPayment)
                throw new ValidationError("PaymentTransaction: tfPartialPayment flag required with DeliverMin");
            if (!Common.IsAmount(DeliverMin))
                throw new ValidationError("PaymentTransaction: invalid DeliverMin");
            return Task.CompletedTask;
        }

        public static bool IsPathStep(dynamic pathStep)
        {
            if (pathStep.ContainsKey("account") && pathStep["account"] is not string { } account)
                return false;
            if (pathStep.ContainsKey("currency") && pathStep["currency"] is not string { } currency)
                return false;
            if (pathStep.ContainsKey("issuer") && pathStep["issuer"] is not string { } issuer)
                return false;

            if (pathStep.ContainsKey("account") && !pathStep.ContainsKey("currency") && !pathStep.ContainsKey("issuer"))
                return true;
            if (pathStep.ContainsKey("currency") || pathStep.ContainsKey("issuer"))
                return true;
            return false;
        }
        public static bool IsPaths(Dictionary<string, dynamic> paths)
        {
            foreach (var path in paths)
            {
                if (!IsPathStep(path))
                    return false;
            }

            return true;

        }
        public static bool IsPaths(List<List<Dictionary<string, dynamic>>> paths)
        {
            if (paths is null || paths.Count == 0)
                return false;
            foreach (var c in paths)
            {
                if (c is null || c.Count == 0) return false;
                if (!IsPaths(paths)) return false;
            }
            return true;
        }
    }

}
