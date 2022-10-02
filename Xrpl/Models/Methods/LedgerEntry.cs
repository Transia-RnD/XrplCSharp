using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xrpl.Models.Ledger;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/ledgerEntry.ts

namespace Xrpl.Models.Methods
{
    //https://xrpl.org/ledger_entry.html
    public enum LedgerEntryRequestType //todo not full enum (index account_root directory offer ripple_state check escrow payment_channel deposit_preauth ticket)
    {
        [EnumMember(Value = "index")]
        Index,
        [EnumMember(Value = "account_root")]
        AccountRoot,
        [EnumMember(Value = "directory")]
        Directory,
        [EnumMember(Value = "offer")]
        Offer,
        [EnumMember(Value = "ripple_state")]
        RippleState
    }
    /// <summary>
    /// The `ledger_entry` method returns a single ledger object from the XRP Ledger  in its raw format.<br/>
    /// Expects a response in the form of a <see cref="LOLedgerEntry"/>.
    /// </summary>
    /// <code>
    ///  ```ts  const ledgerEntry: LedgerEntryRequest ={
    /// 	command: "ledger_entry",
    /// 	ledger_index: 60102302,
    /// 	index: "7DB0788C020F02780A673DC74757F23823FA3014C1866E72CC4CD8B226CD6EF4"}
    /// ```
    /// </code>
    public class LedgerEntryRequest : BaseLedgerRequest
    {
        public LedgerEntryRequest()
        {
            Command = "ledger_entry";
        }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LedgerEntryRequestType LedgerEntryRequestType { get; set; }

        /// <summary>
        /// Only one of the following properties should be defined in a single request.<br/>
        /// org/ledger_entry.<br/>
        /// html.<br/>
        /// Retrieve any type of ledger object by its unique ID.
        /// </summary>
        [JsonProperty("index")]
        public string Index { get; set; }

        /// <summary>
        /// Retrieve an AccountRoot object by its address.<br/>
        /// This is roughly equivalent to the an {@link AccountInfoRequest}.
        /// </summary>
        [JsonProperty("account_root")]
        public string AccountRoot { get; set; }

        /// <summary>
        /// Object specifying the RippleState (trust line) object to retrieve.<br/>
        /// The accounts and currency sub-fields are required to uniquely specify the rippleState entry to retrieve.
        /// </summary>
        [JsonProperty("ripple_state")]
        public RippleState RippleState { get; set; }
        /// <summary>
        /// If true, return the requested ledger object's contents as a hex string in the XRP Ledger's binary format.<br/>
        /// Otherwise, return data in JSON format.<br/>
        /// The default is false.
        /// </summary>
        [JsonProperty("binary")]
        public bool? Binary { get; set; }

        //todo not found fields -  directory?:, offer?:, check?: string, escrow?:, payment_channel?: string,  deposit_preauth?:, ticket?:
    }

    /// <summary>
    /// Object specifying the RippleState (trust line) object to retrieve.<br/>
    /// The accounts and currency sub-fields are required to uniquely specify the rippleState entry to retrieve.
    /// </summary>
    public class RippleState
    {
        /// <summary>
        /// 2-length array of account Addresses, defining the two accounts linked by  this RippleState object.
        /// </summary>
        [JsonProperty("accounts")]
        public string[] Addresses { get; set; }

        /// <summary>
        /// Currency Code of the RippleState object to retrieve.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }        
    }
}
