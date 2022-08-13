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
                "NFTokenMint" => new NFTokenMintTransactionResponse(),
                "NFTokenBurn" => new NFTokenBurnTransactionResponse(),
                "NFTokenCreateOffer" => new NFTokenCreateOfferTransactionResponse(),
                "NFTokenCancelOffer" => new NFTokenCancelOfferTransactionResponse(),
                "NFTokenAcceptOffer" => new NFTokenAcceptOfferTransactionResponse(),
                "AccountSet" => new AccountSetTransactionResponse(),
                "EscrowCancel" => new EscrowCancelTransactionResponse(),
                "EscrowCreate" => new EscrowCreateTransactionResponse(),
                "EscrowFinish" => new EscrowFinishTransactionResponse(),
                "OfferCancel" => new OfferCancelTransactionResponse(),
                "OfferCreate" => new OfferCreateTransactionResponse(),
                "Payment" => new PaymentTransactionResponse(),
                "PaymentChannelClaim" => new PaymentChannelClaimTransactionResponse(),
                "PaymentChannelCreate" => new PaymentChannelCreateTransactionResponse(),
                "PaymentChannelFund" => new PaymentChannelFundTransactionResponse(),
                "SetRegularKey" => new SetRegularKeyTransactionResponse(),
                "SignerListSet" => new SignerListSetTransactionResponse(),
                "TrustSet" => new TrustSetTransactionResponse(),
                "EnableAmendment" => new EnableAmendmentTransactionResponse(),
                "SetFee" => new SetFeeTransactionResponse(),
                "AccountDelete" => new AccountDeleteTransactionResponse(),
                "TicketCreate" => new TicketCreateTransactionResponse(),
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
