using Newtonsoft.Json;

using System;
using Xrpl.Client.Json.Converters;

//https://xrpl.org/channel_authorize.html#channel_authorize

namespace Xrpl.Models.Methods
{
    /// <summary>
    /// The channel_authorize method creates a signature that can be used to redeem a specific amount of XRP from a payment channel.<br/>
    /// The request must specify exactly one of secret, seed, seed_hex, or passphrase.
    /// <b>*** Warning: Do not send secret keys to untrusted servers or through unsecured network connections. ***</b><br/>
    /// (This includes the secret, seed, seed_hex, or passphrase fields of this request.))<br/>
    /// You should only use this method on a secure, encrypted network connection to a server you run or fully trust with your funds.<br/>
    /// Otherwise, eavesdroppers could use your secret key to sign claims and take all the money from this payment channel and anything else using the same key pair.<br/>
    /// See Set Up Secure Signing for instructions.<br/>
    /// </summary>
    /// <code>
    /// {
    /// 	"id": "channel_authorize_example_id1",
    /// 	"command": "channel_authorize",
    /// 	"channel_id": "5DB01B7FFED6B67E6B0414DED11E051D2EE2B7619CE0EAA6286D67A3A4D5BDB3",
    /// 	"seed": "s████████████████████████████",
    /// 	"key_type": "secp256k1",
    /// 	"amount": "1000000",
    /// }
    /// </code>
    public class ChannelAuthorizeRequest : BaseRequest
    {
        public ChannelAuthorizeRequest()
        {
            Command = "channel_authorize";
        }
        /// <summary>
        /// The unique ID of the payment channel to use.
        /// </summary>
        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }

        /// <summary>
        /// <b>*** Do not send your secret to a server that you do not control or do not trust. ***</b><br/>
        /// (Optional) The secret key to use to sign the claim.<br/>
        /// This must be the same key pair as the public key specified in the channel.<br/>
        /// Cannot be used with seed, seed_hex, or passphrase.
        /// </summary> 
        [JsonProperty("secret")]
        public string Secret { get; set; }
        /// <summary>
        /// <b>*** Do not send your secret to a server that you do not control or do not trust. ***</b><br/>
        /// (Optional) The secret seed to use to sign the claim.<br/>
        /// This must be the same key pair as the public key specified in the channel.<br/>
        /// Must be in the XRP Ledger's base58 format.<br/>
        /// If provided, you must also specify the key_type.<br/>
        /// Cannot be used with secret, seed_hex, or passphrase.
        /// </summary>
        [JsonProperty("seed")]
        public string Seed { get; set; }
        /// <summary>
        /// <b>*** Do not send your secret to a server that you do not control or do not trust. ***</b><br/>
        /// (Optional) A string passphrase to use to sign the claim.<br/>
        /// This must be the same key pair as the public key specified in the channel.<br/>
        /// The key derived from this passphrase must match the public key specified in the channel.<br/>
        /// If provided, you must also specify the key_type.<br/>
        /// Cannot be used with secret, seed, or seed_hex.
        /// </summary>
        [JsonProperty("passphrase")]
        public string Passphrase { get; set; }
        /// <summary>
        /// <b>*** Do not send your secret to a server that you do not control or do not trust. ***</b><br/>
        /// (Optional) The signing algorithm of the cryptographic key pair provided.<br/>
        /// Valid types are secp256k1 or ed25519.<br/>
        /// The default is secp256k1.
        /// </summary>
        [JsonProperty("key_type")]
        public string KeyType { get; set; }

        /// <summary>
        /// Cumulative amount of XRP, in drops, to authorize.<br/>
        /// If the destination has already received a lesser amount of XRP from this channel, the signature created by this method can be redeemed for the difference.
        /// </summary>
        [JsonProperty("amount")]
        [JsonConverter(typeof(GenericStringConverter<ulong>))]
        public ulong Amount { get; set; }

        [JsonIgnore]
        public double RippleAmount
        {
            get => (double) Amount / 1000000;
            set => Amount =  Convert.ToUInt32(value * 1000000);
        }
    }
}
