using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

using Xrpl.Client.Models.Transactions;

namespace Xrpl.Client.Json.Converters
{
    public class TransactionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public ITransactionResponseCommon Create(Type objectType, JObject jObject)
        {
            var transactionType = jObject.Property("TransactionType");

            return jObject.Property("TransactionType").Value.ToString() switch
            {
                "AccountSet" => new AccountSetResponse(),
                "AccountDelete" => new AccountDeleteResponse(),
                "CheckCancel" => new AccountDeleteResponse(),
                "CheckCash" => new AccountDeleteResponse(),
                "CheckCreate" => new AccountDeleteResponse(),
                "DepositPreauth" => new DepositPreauthResponse(),
                "EscrowCancel" => new EscrowCancelResponse(),
                "EscrowCreate" => new EscrowCreateResponse(),
                "EscrowFinish" => new EscrowFinishResponse(),
                "NFTokenAcceptOffer" => new NFTokenAcceptOfferResponse(),
                "NFTokenCancelOffer" => new NFTokenCancelOfferResponse(),
                "NFTokenBurn" => new NFTokenBurnResponse(),
                "NFTokenCreateOffer" => new NFTokenCreateOfferResponse(),
                "NFTokenMint" => new NFTokenMintResponse(),
                "OfferCancel" => new OfferCancelResponse(),
                "OfferCreate" => new OfferCreateResponse(),
                "Payment" => new PaymentResponse(),
                "PaymentChannelClaim" => new PaymentChannelClaimResponse(),
                "PaymentChannelCreate" => new PaymentChannelCreateResponse(),
                "PaymentChannelFund" => new PaymentChannelFundResponse(),
                "SetRegularKey" => new SetRegularKeyResponse(),
                "SignerListSet" => new SignerListSetResponse(),
                "TicketCreate" => new TicketCreateResponse(),
                "TrustSet" => new TrustSetResponse(),
                _ => throw new Exception("Can't create transaction type" + transactionType)
            };
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            ITransactionResponseCommon transactionCommon = Create(objectType, jObject);
            serializer.Populate(jObject.CreateReader(), transactionCommon);
            return transactionCommon;            
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(ITransactionResponseCommon);

        public override bool CanWrite => false;
    }
}