using System;
using Newtonsoft.Json;
using Xrpl.ClientLib.Json.Converters;

//https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/methods/channelVerify.ts

namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The `channel_verify` method checks the validity of a signature that can be  used to redeem a specific amount of XRP from a payment channel.<br/>
    /// Expects a  response in the form of a ChannelVerifyResponse.
    /// </summary>
    public class ChannelVerifyRequest : RippleRequest
    {
        public ChannelVerifyRequest()
        {
            Command = "channel_verify";
        }
        /// <summary>
        /// The Channel ID of the channel that provides the XRP.<br/>
        /// This is a 64-character hexadecimal string.
        /// </summary>
        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }
        /// <summary>
        /// The signature to verify, in hexadecimal.
        /// </summary>
        [JsonProperty("signature")]
        public string Signature { get; set; }
        /// <summary>
        /// The public key of the channel and the key pair that was used to create the signature, in hexadecimal or the XRP Ledger's base58 format.
        /// </summary>
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }
        /// <summary>
        /// The amount of XRP, in drops, the provided signature authorizes.
        /// </summary>
        [JsonProperty("amount")]
        [JsonConverter(typeof(GenericStringConverter<ulong>))]
        public ulong Amount { get; set; }

        [JsonIgnore]
        public double RippleAmount
        {
            get => (double)Amount / 1000000;
            set => Amount = Convert.ToUInt32(value * 1000000);
        }
    }
    //todo not found ChannelVerifyResponse https://github.com/XRPLF/xrpl.js/blob/b20c05c3680d80344006d20c44b4ae1c3b0ffcac/packages/xrpl/src/models/methods/channelVerify.ts#L33
}
