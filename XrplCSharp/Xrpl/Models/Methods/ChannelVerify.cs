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
    /// <code>
    /// {
    /// 	"id": 1,
    /// 	"command": "channel_verify",
    /// 	"channel_id": "5DB01B7FFED6B67E6B0414DED11E051D2EE2B7619CE0EAA6286D67A3A4D5BDB3",
    /// 	"signature": "304402204EF0AFB78AC23ED1C472E74F4299C0C21F1B21D07EFC0A3838A420F76D783A400220154FB11B6F54320666E4C36CA7F686C16A3A0456800BBC43746F34AF50290064",
    /// 	"public_key": "aB44YfzW24VDEJQ2UuLPV2PvqcPCSoLnL7y5M1EzhdW4LnK5xMS3",
    /// 	"amount": "1000000"
    /// }
    /// </code>
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
