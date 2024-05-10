using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Xrpl.BinaryCodec.Types;
using Xrpl.Models.Ledger;
using Xrpl.Client.Extensions;

//https://xrpl.org/ledger-header.html#ledger-index
//https://xrpl.org/currency-formats.html#currency-formats
// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/common/index.ts

namespace Xrpl.Models.Common
{
    /// <summary>
    /// The XRP Ledger has two kinds of digital asset: XRP and tokens.<br/>
    /// Both types have high precision, although their formats are different
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// base constructor.<br/>
        /// base currency code = XRP
        /// </summary>
        public Currency()
        {
            CurrencyCode = "XRP";
        }

        /// <summary>
        /// The standard format for currency codes is a three-character string such as USD.<br/>
        /// This is intended for use with ISO 4217 Currency Codes <br/>
        /// As a 160-bit hexadecimal string, such as "0158415500000000C1F76FF6ECB0BAC600000000".<br/>
        /// The following characters are permitted:<br/>
        /// all uppercase and lowercase letters, digits, as well as the symbols ? ! @ # $ % ^ * ( ) { } [ ] | and symbols ampersand, less, greater<br/>
        /// Currency codes are case-sensitive.
        /// </summary>
        [JsonProperty("currency")]
        public string CurrencyCode { get; set; }
        /// <summary>
        /// Quoted decimal representation of the amount of the token.<br/>
        /// This can include scientific notation, such as 1.23e11 meaning 123,000,000,000.<br/>
        /// Both e and E may be used.<br/>
        /// This can be negative when displaying balances, but negative values are disallowed in other contexts such as specifying how much to send.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
        /// <summary>
        /// Generally, the account that issues this token.<br/>
        /// In special cases, this can refer to the account that holds the token instead.
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
        /// <summary>
        /// Readable currency name 
        /// </summary>
        [JsonIgnore]
        public string CurrencyValidName => CurrencyCode is { Length: > 0 } row ? row.Length > 3 ? row.FromHexString().Trim('\0') : row : string.Empty;
        /// <summary>
        /// decimal currency amount (drops for XRP)
        /// </summary>
        [JsonIgnore]
        public decimal ValueAsNumber
        {
            get => string.IsNullOrWhiteSpace(Value)
                ? 0
                : decimal.Parse(Value, 
                    NumberStyles.AllowLeadingSign 
                    | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent)
                    | (NumberStyles.AllowLeadingSign & NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
                    | (NumberStyles.AllowExponent & NumberStyles.AllowDecimalPoint)
                    | NumberStyles.AllowExponent
                    | NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture);

            set => Value = value.ToString(CurrencyCode == "XRP"
                ? "G0"
                : "G15",
                CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// XRP token amount (non drops value)
        /// </summary>
        [JsonIgnore]
        public decimal? ValueAsXrp
        {
            get
            {
                if (CurrencyCode != "XRP" || string.IsNullOrWhiteSpace(Value))
                    return null;
                return ValueAsNumber / 1000000;
            }
            set
            {
                if (value.HasValue)
                {
                    CurrencyCode = "XRP";
                    decimal val = value.Value * 1000000;
                    Value = val.ToString("G0", CultureInfo.InvariantCulture);
                }
                else
                {
                    Value = "0";
                }
            }
        }
        #region Overrides of Object

        public override string ToString() => CurrencyValidName == "XRP" ? $"XRP: {ValueAsXrp:0.######}" : $"{CurrencyValidName}: {ValueAsNumber:0.###############}";
        public override bool Equals(object o) => o is Currency model && model.Issuer == Issuer && model.CurrencyCode == CurrencyCode;

        public static bool operator ==(Currency c1, Currency c2) => c1.Equals(c2);

        public static bool operator !=(Currency c1, Currency c2) => !c1.Equals(c2);

        #endregion

    }

    public class LedgerIndex
    {
        public LedgerIndex(uint index)
        {
            Index = index;
        }

        public LedgerIndex(LedgerIndexType ledgerIndexType)
        {
            LedgerIndexType = ledgerIndexType;
        }

        public uint? Index { get; set; }
        /// <summary>
        /// Index type<br/>
        /// validated<br/>
        /// closed<br/>
        /// current<br/>
        /// </summary>
        public LedgerIndexType LedgerIndexType { get; set; }
    }

    /// <summary> common class </summary>
    public class Common
    {
        public enum LedgerIndex
        {
            Validated,
            Closed,
            Current
        }

        public enum AccountObjectType
        {
            Check,
            DepositPreauth,
            Escrow,
            NftOffer,
            Offer,
            PaymentChannel,
            SignerList,
            Ticket,
            State
        }

        public class MemoEntry
        {
            public string MemoData { get; set; }
            public string MemoType { get; set; }
            public string MemoFormat { get; set; }
        }

        public class ISigner
        {
            public SignerEntry Signer { get; set; }
        }

        public class IssuedCurrencyAmount
        {
            [JsonProperty("currency")]
            public string Currency { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }

            [JsonProperty("issuer")]
            public string Issuer { get; set; }
        }

        public class IMemo
        {
            public MemoEntry Memo { get; set; }
        }

        public enum StreamType
        {
            Consensus,
            Ledger,
            Manifests,
            PeerStatus,
            Transactions,
            TransactionsProposed,
            Server,
            Validations
        }

        public class PathStep
        {
            public string Account { get; set; }
            public string Currency { get; set; }
            public string Issuer { get; set; }
        }

        public class Path
        {
            public List<PathStep> PathStep { get; set; }
        }

        public class ResponseOnlyTxInfo
        {
            public int Date { get; set; }
            public string Hash { get; set; }
            public int LedgerIndex { get; set; }
        }

        public class NFTOffer
        {
            public Amount Amount { get; set; }
            public int Flags { get; set; }
            public string NftOfferIndex { get; set; }
            public string Owner { get; set; }
            public string Destination { get; set; }
            public int Expiration { get; set; }
        }
    }
}

