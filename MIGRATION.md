# Migration

## From RippleDotNet & ripple-netcore to version 1.x

### Client previously known as (RippleDotNet or Ripple.Core)

From RippleDotNet

```
using RippleDotNet;
using RippleDotNet.Model.Account;
using RippleDotNet.Requests.Account;
```

To xrpl.c

```
using Xrpl;
using Xrpl.Model.Account;
using Xrpl.Requests.Account;
```

From Ripple.Core

```
using Ripple.Core.Types;
```

To xrpl.c

```
using Xrpl.BinaryCodecLib.Types;
```

### Wallet previously known as (Ripple.Signing)

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
using Xrpl.KeypairsLib;
```