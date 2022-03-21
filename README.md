[![NuGet Badge](https://buildstats.info/nuget/xrpl.c)](https://www.nuget.org/packages/xrpl.c/)

# xrpl.c

* This library would not be possible without Chris Williams.

A pure C# implementation for interacting with the XRP Ledger, the `xrpl.c` library simplifies the hardest parts of XRP Ledger interaction, like serialization and transaction signing, by providing native C# methods and models for [XRP Ledger transactions](https://xrpl.org/transaction-formats.html) and core server [API](https://xrpl.org/api-conventions.html) ([`rippled`](https://github.com/ripple/rippled)) objects.

## Migration from RippleDotNet & ripple-netcore

From RippleDotNet

```
using RippleDotNet;
using RippleDotNet.Model.Account;
using RippleDotNet.Requests.Account;
```

To xrpl.c

```
using Xrpl.Client;
using Xrpl.Client.Model.Account;
using Xrpl.Client.Requests.Account;
```

From Ripple.Core

```
using Ripple.Core.Types;
```

To xrpl.c

```
using Ripple.Binary.Codec.Types;
```


From Ripple.Signing

```
using Ripple.Signing;
```

To xrpl.c

```
using Xrpl.Wallet;
```


From Ripple.TxSigning

```
using Ripple.TxSigning;
```

To xrpl.c

```
using Ripple.Keypairs;
```




```csharp
# create a network client
using Xrpl.Client;
IRippleClient client = new RippleClient("wss://s.altnet.rippletest.net:51233");
client.Connect();

# create a wallet on the testnet
using Xrpl.Wallet;
using System.Diagnostics;
// test_wallet = SOON
Debug.WriteLine(testWallet);
public_key: ED3CC1BBD0952A60088E89FA502921895FC81FBD79CAE9109A8FE2D23659AD5D56
private_key: -HIDDEN-
classic_address: rBtXmAdEYcno9LWRnAGfT9qBxCeDvuVRZo

# look up account info
using System.Diagnostics;
using Xrpl.Client.Model.Account;
AccountInfo accountInfo = await client.AccountInfo(account);
Debug.WriteLine(accountInfo);
# {
#     "Account": "rBtXmAdEYcno9LWRnAGfT9qBxCeDvuVRZo",
#     "Balance": "1000000000",
#     "Flags": 0,
#     "LedgerEntryType": "AccountRoot",
#     "OwnerCount": 0,
#     "PreviousTxnID": "73CD4A37537A992270AAC8472F6681F44E400CBDE04EC8983C34B519F56AB107",
#     "PreviousTxnLgrSeq": 16233962,
#     "Sequence": 16233962,
#     "index": "FD66EC588B52712DCE74831DCB08B24157DC3198C29A0116AA64D310A58512D7"
# }
```

<!-- [![Downloads](https://pepy.tech/badge/xrpl-py/month)](https://pepy.tech/project/xrpl-py/month)
[![Contributors](https://img.shields.io/github/contributors/xpring-eng/xrpl-py.svg)](https://github.com/xpring-eng/xrpl-py/graphs/contributors) -->

## Installation and supported versions

The `xrpl.c` library is available on [DotNet](https://dotnet.microsoft.com/). Install with `dotnet`:


```
dotnet add package xrpl.c --version 1.0.0
```

The library supports [Dotnet 5](https://dotnet.microsoft.com/) and later.

<!-- [![Supported Versions](https://img.shields.io/pypi/pyversions/xrpl.c.svg)](https://pypi.org/project/xrpl-py) -->


## Features

Use `xrpl.c` to build C# applications that leverage the [XRP Ledger](https://xrpl.org/). The library helps with all aspects of interacting with the XRP Ledger, including:

* Key and wallet management
* Serialization
* Transaction Signing

`xrpl.c` also provides:

* A network client — See [`xrpl.clients`](https://xrpl-c.readthedocs.io/en/stable/source/xrpl.clients.html) for more information.
* Methods for inspecting accounts — See [XRPL Account Methods](https://xrpl-c.readthedocs.io/en/stable/source/xrpl.account.html) for more information.
* Codecs for encoding and decoding addresses and other objects — See [Core Codecs](https://xrpl-c.readthedocs.io/en/stable/source/xrpl.core.html) for more information.

## [➡️ Reference Documentation](https://xrpl-c.readthedocs.io/en/stable/)

See the complete [`xrpl.c` reference documentation on Read the Docs](https://xrpl-c.readthedocs.io/en/stable/index.html).


## Usage

The following sections describe some of the most commonly used modules in the `xrpl.c` library and provide sample code.

### Network client

Use the `Xrpl.Client` library to create a network client for connecting to the XRP Ledger.

```csharp
using Xrpl.Client;
IRippleClient client = new RippleClient("wss://s.altnet.rippletest.net:51233");
client.Connect();
```

### Manage keys and wallets

#### `Xrpl.Wallet`

Use the [`Xrpl.Wallet`](https://xrpl-c.readthedocs.io/en/stable/source/xrpl.wallet.html) module to create a wallet from a given seed or or via a [Testnet faucet](https://xrpl.org/xrp-testnet-faucet.html).

To create a wallet from a seed (in this case, the value generated using [`Xrpl.Keypairs`](#xrpl-keypairs)):

```csharp
using Xrpl.Wallet;
using System.Diagnostics;
TxSigner signer = TxSigner.FromSecret(seed);
Debug.WriteLine(signer);
# pub_key: ED46949E414A3D6D758D347BAEC9340DC78F7397FEE893132AAF5D56E4D7DE77B0
# priv_key: -HIDDEN-
# classic_address: rG5ZvYsK5BPi9f1Nb8mhFGDTNMJhEhufn6
```

<!-- To create a wallet from a Testnet faucet:

```csharp
test_wallet = SOON
test_account = test_wallet.classic_address
Debug.WriteLine(test_account);
# Classic address: rEQB2hhp3rg7sHj6L8YyR4GG47Cb7pfcuw
``` -->

#### `Ripple.Keypairs`

Use the [`Ripple.Keypairs`](https://xrpl-c.readthedocs.io/en/stable/source/xrpl.core.keypairs.html#module-xrpl.core.keypairs) module to generate seeds and derive keypairs and addresses from those seed values.

Here's an example of how to generate a `seed` value and derive an [XRP Ledger "classic" address](https://xrpl.org/cryptographic-keys.html#account-id-and-address) from that seed.


```csharp
using Ripple.Keypairs;
using System.Diagnostics;
Seed seed = Seed.FromRandom();
KeyPair pair = seed.KeyPair();
string public = pair.Id();
string private = seed.ToString();
Debug.WriteLine("Here's the public key:");
print("Here's the public key:")
Debug.WriteLine(public);
Debug.WriteLine("Here's the private key:");
Debug.WriteLine(private);
Debug.WriteLine("Store this in a secure place!");
# Here's the public key:
# ED3CC1BBD0952A60088E89FA502921895FC81FBD79CAE9109A8FE2D23659AD5D56
# Here's the private key:
# EDE65EE7882847EF5345A43BFB8E6F5EEC60F45461696C384639B99B26AAA7A5CD
# Store this in a secure place!
```

**Note:** You can use `Ripple.Keypairs` to sign transactions but `xrpl.c` also provides explicit methods for safely signing and submitting transactions. See [Transaction Signing](#transaction-signing) and [XRPL Transaction Methods](https://xrpl.c.readthedocs.io/en/stable/source/xrpl.transaction.html#module-xrpl.transaction) for more information.


### Serialize and sign transactions

To securely submit transactions to the XRP Ledger, you need to first serialize data from JSON and other formats into the [XRP Ledger's canonical format](https://xrpl.org/serialization.html), then to [authorize the transaction](https://xrpl.org/transaction-basics.html#authorizing-transactions) by digitally [signing it](https://xrpl.c.readthedocs.io/en/stable/source/xrpl.core.keypairs.html?highlight=sign#xrpl.core.keypairs.sign) with the account's private key. The `xrpl.c` library provides several methods to simplify this process.


Use the [`xrpl.transaction`](https://xrpl.c.readthedocs.io/en/stable/source/xrpl.transaction.html) module to sign and submit transactions. The module offers three ways to do this:

* [`safe_sign_and_submit_transaction`](https://xrpl.c.readthedocs.io/en/stable/source/xrpl.transaction.html#xrpl.transaction.safe_sign_and_submit_transaction) — Signs a transaction locally, then submits it to the XRP Ledger. This method does not implement [reliable transaction submission](https://xrpl.org/reliable-transaction-submission.html#reliable-transaction-submission) best practices, so only use it for development or testing purposes.

* [`safe_sign_transaction`](https://xrpl.c.readthedocs.io/en/stable/source/xrpl.transaction.html#xrpl.transaction.safe_sign_transaction) — Signs a transaction locally. This method **does  not** submit the transaction to the XRP Ledger.

* [`send_reliable_submission`](https://xrpl.c.readthedocs.io/en/stable/source/xrpl.transaction.html#xrpl.transaction.send_reliable_submission) — An implementation of the [reliable transaction submission guidelines](https://xrpl.org/reliable-transaction-submission.html#reliable-transaction-submission), this method submits a signed transaction to the XRP Ledger and then verifies that it has been included in a validated ledger (or has failed to do so). Use this method to submit transactions for production purposes.


```csharp
using Xrpl.Client.Model.Account;
using Xrpl.Client.Requests.Account;
using Xrpl.Client.Model.Transaction;

AccountInfo accountInfo = await client.AccountInfo("rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V");

# prepare the transaction
# the amount is expressed in drops, not XRP
# see https://xrpl.org/basic-data-types.html#specifying-currency-amounts
IPaymentTransaction paymentTransaction = new PaymentTransaction();
paymentTransaction.Account = "rwEHFU98CjH59UX2VqAgeCzRFU9KVvV71V";
paymentTransaction.Destination = "rEqtEHKbinqm18wQSQGstmqg9SFpUELasT";
paymentTransaction.Amount = new Currency { ValueAsXrp = 1 };
paymentTransaction.Sequence = accountInfo.AccountData.Sequence;

# sign the transaction
TxSigner signer = TxSigner.FromSecret("xxxxxxx");  //secret is not sent to server, offline signing only
SignedTx signedTx = signer.SignJson(JObject.Parse(paymentTransaction.ToJson()));

# submit the transaction
SubmitBlobRequest request = new SubmitBlobRequest();
request.TransactionBlob = signedTx.TxBlob;

Submit result = await client.SubmitTransactionBlob(request);
```

#### Get fee from the XRP Ledger


In most cases, you can specify the minimum [transaction cost](https://xrpl.org/transaction-cost.html#current-transaction-cost) of `"10"` for the `fee` field unless you have a strong reason not to. But if you want to get the [current load-balanced transaction cost](https://xrpl.org/transaction-cost.html#current-transaction-cost) from the network, you can use the `Fees` function:

```csharp
using System.Diagnostics;
Fee fee = await client.Fees();
Debug.WriteLine(fee);
# 10
```

<!-- #### Auto-filled fields

The `xrpl.c` library automatically populates the `fee`, `sequence` and `last_ledger_sequence` fields when you create transactions. In the example above, you could omit those fields and let the library fill them in for you.

```csharp
from xrpl.models.transactions import Payment
from xrpl.transaction import send_reliable_submission, safe_sign_and_autofill_transaction
# prepare the transaction
# the amount is expressed in drops, not XRP
# see https://xrpl.org/basic-data-types.html#specifying-currency-amounts
my_tx_payment = Payment(
    account=test_wallet.classic_address,
    amount="2200000",
    destination="rPT1Sjq2YGrBMTttX4GZHjKu9dyfzbpAYe"
)

# sign the transaction with the autofill method
# (this will auto-populate the fee, sequence, and last_ledger_sequence)
my_tx_payment_signed = safe_sign_and_autofill_transaction(my_tx_payment, test_wallet, client)
print(my_tx_payment_signed)
# Payment(
#     account='rMPUKmzmDWEX1tQhzQ8oGFNfAEhnWNFwz',
#     transaction_type=<TransactionType.PAYMENT: 'Payment'>,
#     fee='10',
#     sequence=16034065,
#     account_txn_id=None,
#     flags=0,
#     last_ledger_sequence=10268600,
#     memos=None,
#     signers=None,
#     source_tag=None,
#     signing_pub_key='EDD9540FA398915F0BCBD6E65579C03BE5424836CB68B7EB1D6573F2382156B444',
#     txn_signature='938FB22AE7FE76CF26FD11F8F97668E175DFAABD2977BCA397233117E7E1C4A1E39681091CC4D6DF21403682803AB54CC21DC4FA2F6848811DEE10FFEF74D809',
#     amount='2200000',
#     destination='rPT1Sjq2YGrBMTttX4GZHjKu9dyfzbpAYe',
#     destination_tag=None,
#     invoice_id=None,
#     paths=None,
#     send_max=None,
#     deliver_min=None
# )

# submit the transaction
tx_response = send_reliable_submission(my_tx_payment_signed, client)
``` -->


<!-- ### Subscribe to ledger updates

You can send `subscribe` and `unsubscribe` requests only using the WebSocket network client. These request methods allow you to be alerted of certain situations as they occur, such as when a new ledger is declared.

```csharp
from xrpl.clients import WebsocketClient
url = "wss://s.altnet.rippletest.net/"
from xrpl.models.requests import Subscribe, StreamParameter
req = Subscribe(streams=[StreamParameter.LEDGER])
# NOTE: this code will run forever without a timeout, until the process is killed
with WebsocketClient(url) as client:
    client.send(req)
    for message in client:
        print(message)
# {'result': {'fee_base': 10, 'fee_ref': 10, 'ledger_hash': '7CD50477F23FF158B430772D8E82A961376A7B40E13C695AA849811EDF66C5C0', 'ledger_index': 18183504, 'ledger_time': 676412962, 'reserve_base': 20000000, 'reserve_inc': 5000000, 'validated_ledgers': '17469391-18183504'}, 'status': 'success', 'type': 'response'}
# {'fee_base': 10, 'fee_ref': 10, 'ledger_hash': 'BAA743DABD168BD434804416C8087B7BDEF7E6D7EAD412B9102281DD83B10D00', 'ledger_index': 18183505, 'ledger_time': 676412970, 'reserve_base': 20000000, 'reserve_inc': 5000000, 'txn_count': 0, 'type': 'ledgerClosed', 'validated_ledgers': '17469391-18183505'}
# {'fee_base': 10, 'fee_ref': 10, 'ledger_hash': 'D8227DAF8F745AE3F907B251D40B4081E019D013ABC23B68C0B1431DBADA1A46', 'ledger_index': 18183506, 'ledger_time': 676412971, 'reserve_base': 20000000, 'reserve_inc': 5000000, 'txn_count': 0, 'type': 'ledgerClosed', 'validated_ledgers': '17469391-18183506'}
# {'fee_base': 10, 'fee_ref': 10, 'ledger_hash': 'CFC412B6DDB9A402662832A781C23F0F2E842EAE6CFC539FEEB287318092C0DE', 'ledger_index': 18183507, 'ledger_time': 676412972, 'reserve_base': 20000000, 'reserve_inc': 5000000, 'txn_count': 0, 'type': 'ledgerClosed', 'validated_ledgers': '17469391-18183507'}
``` -->

<!-- ### Encode addresses

Use [`xrpl.core.addresscodec`](https://xrpl-py.readthedocs.io/en/stable/source/xrpl.core.addresscodec.html) to encode and decode addresses into and from the ["classic" and X-address formats](https://xrpl.org/accounts.html#addresses).

```csharp
# convert classic address to x-address
from xrpl.core import addresscodec
testnet_xaddress = (
    addresscodec.classic_address_to_xaddress(
        "rMPUKmzmDWEX1tQhzQ8oGFNfAEhnWNFwz",
        tag=0,
        is_test_network=True,
    )
)
Debug.WriteLine(testnet_xaddress);
# T7QDemmxnuN7a52A62nx2fxGPWcRahLCf3qaswfrsNW9Lps
``` -->


## Contributing

If you want to contribute to this project, see [CONTRIBUTING.md].

### Mailing Lists

We have a low-traffic mailing list for announcements of new `xrpl.c` releases. (About 1 email per week)

+ [Subscribe to xrpl-announce](https://groups.google.com/g/xrpl-announce)

If you're using the XRP Ledger in production, you should run a [rippled server](https://github.com/ripple/rippled) and subscribe to the ripple-server mailing list as well.

+ [Subscribe to ripple-server](https://groups.google.com/g/ripple-server)


## License

The `xrpl.c` library is licensed under the ISC License. See [LICENSE] for more information.



[CONTRIBUTING.md]: CONTRIBUTING.md
[LICENSE]: LICENSE

## Repository Credit

The credit of this repository goes to Chris Williams.

https://github.com/chriswill

Thank you Chris.
