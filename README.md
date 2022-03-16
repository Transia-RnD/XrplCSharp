[![NuGet Badge](https://buildstats.info/nuget/Ripple.APICore)](https://www.nuget.org/packages/Ripple.APICore/)

# Ripple.APICore
A C# NetStandard 2.0 client implementation for the [Ripple WebSocket APIs](https://ripple.com/build/rippled-apis/#websocket-api).

This library is written to the NetStandard2 specification, which means that it can run using .Net Core on Windows, Mac OS/X and Unix.  I'm only testing it on Windows however, so I'd appreciate any feedback on other platforms.

This library is in the early stages of development and should only be used on the TestNet. Use at your own risk.

## Install
Install from Nuget --> https://www.nuget.org/packages/Ripple.APICore/

## Examples

### Get Account Information
```csharp
IRippleClient client = new RippleClient("wss://s.altnet.rippletest.net:51233");
client.Connect();
RippleDotNet.Model.Accounts.AccountInfo accountInfo = await client.AccountInfo("rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V");
client.Disconnect();
```

### Send A Payment

Note that this request sends your Secret Key to the server.  You should never do this for a server you do not control or do not trust since this can expose your secret key.
To send a payment using offline signing, see the example following this one which uses the [ripple-netcore](https://github.com/chriswill/ripple-netcore) library.

```csharp
IRippleClient client = new RippleClient("wss://s.altnet.rippletest.net:51233");
client.Connect();

IPaymentTransaction paymentTransaction = new PaymentTransaction();
paymentTransaction.Account = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";
paymentTransaction.Destination = "rEqtEHKbinqm18wQSQGstmqg9SFpUELasT";
paymentTransaction.Amount = new Currency { CurrencyCode = "XRP", Value = "100000" };

SubmitRequest request = new SubmitRequest();
request.Transaction = paymentTransaction;
request.Offline = false;
request.Secret = "xxxxxxx";

Submit result = await client.SubmitTransaction(request);

client.Disconnect();
```

### Send A Payment using Offline Signing

TxSigner is a class from [ripple-netcore](https://github.com/chriswill/ripple-netcore), mentioned above.

```csharp
IRippleClient client = new RippleClient("wss://s.altnet.rippletest.net:51233");
client.Connect();

AccountInfo accountInfo = await client.AccountInfo("rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V");

IPaymentTransaction paymentTransaction = new PaymentTransaction();
paymentTransaction.Account = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";
paymentTransaction.Destination = "rEqtEHKbinqm18wQSQGstmqg9SFpUELasT";
paymentTransaction.Amount = new Currency { ValueAsXrp = 1 };
paymentTransaction.Sequence = accountInfo.AccountData.Sequence;

TxSigner signer = TxSigner.FromSecret("xxxxxxx");  //secret is not sent to server, offline signing only
SignedTx signedTx = signer.SignJson(JObject.Parse(paymentTransaction.ToJson()));

SubmitBlobRequest request = new SubmitBlobRequest();
request.TransactionBlob = signedTx.TxBlob;

Submit result = await client.SubmitTransactionBlob(request);

client.Disconnect();
```

You can see additional examples, including Creating Offers, etc. by looking at the unit test project.
  
