[![NuGet Badge](https://buildstats.info/nuget/Ripple.APICore)](https://www.nuget.org/packages/Ripple.APICore/)

# Xrpl.C
A C# NetStandard 5.0 client implementation for the [Ripple WebSocket APIs](https://ripple.com/build/rippled-apis/#websocket-api).

This library is written to the NetStandard5 specification, which means that it can run using .Net Core on Windows, Mac OS/X and Unix.  I'm only testing it on Windows however, so I'd appreciate any feedback on other platforms.

This library is in the early stages of development and should only be used on the TestNet. Use at your own risk.

## Install
Install from Nuget --> https://www.nuget.org/packages/Xrpl.C/

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

## Transaction signing

Signing is done with either ecdsa/rfc6979 or ed25519. See [ripple-keypairs](https://github.com/ripple/ripple-keypairs) for how to generate a seed/secret, encoded in base58.

```c#

// using Ripple.TxSigning
// using Newtonsoft.Json.Linq;

var secret = "sEd7rBGm5kxzauRTAV2hbsNz7N45X91";
var unsignedTxJson = @"{
    'Account': 'rJZdUusLDtY9NEsGea7ijqhVrXv98rYBYN',
    'Amount': '1000',
    'Destination': 'rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh',
    'Fee': '10',
    'Flags': 2147483648,
    'Sequence': 1,
    'TransactionType' : 'Payment'
}";

var signed = TxSigner.SignJson(JObject.Parse(unsignedTxJson), secret);

Console.WriteLine(signed.Hash);
Console.WriteLine(signed.TxJson);
Console.WriteLine(signed.TxBlob);

// A8A9C869671D35A18DFB69AFB7741062DF43F73C8A5942AD94EE58ED31477AC6
// {
//   "Account": "rJZdUusLDtY9NEsGea7ijqhVrXv98rYBYN",
//   "Amount": "1000",
//   "Destination": "rHb9CJAWyB4rj91VRWn96DkukG4bwdtyTh",
//   "Fee": "10",
//   "Flags": 2147483648,
//   "Sequence": 1,
//   "TransactionType": "Payment",
//   "SigningPubKey": "EDD3993CDC6647896C455F136648B7750723B011475547AF60691AA3D7438E021D",
//   "TxnSignature": "C3646313B08EED6AF4392261A31B961F10C66CB733DB7F6CD9EAB079857834C8B0334270A2C037E63CDCCC1932E0832882B7B7066ECD2FAEDEB4A83DF8AE6303"
// }
// 120000228000000024000000016140000000000003E868400000000000000A7321EDD3993CDC6647896C455F136648B7750723B011475547AF60691AA3D7438E021D7440C3646313B08EED6AF4392261A31B961F10C66CB733DB7F6CD9EAB079857834C8B0334270A2C037E63CDCCC1932E0832882B7B7066ECD2FAEDEB4A83DF8AE63038114C0A5ABEF242802EFED4B041E8F2D4A8CC86AE3D18314B5F762798A53D543A014CAF8B297CFF8F2F937E8
