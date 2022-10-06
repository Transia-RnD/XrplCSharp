using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xrpl.Client.Json.Converters;
using Xrpl.Models.Common;
using Xrpl.Models.Methods;

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
}
