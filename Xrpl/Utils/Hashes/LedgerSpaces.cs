

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/hashes/ledgerSpaces.ts

namespace Xrpl.Utils.Hashes
{
    /// <summary>
    /// XRP Ledger namespace prefixes.
    /// The XRP Ledger is a key-value store.In order to avoid name collisions,
    /// names are partitioned into namespaces.
    /// Each namespace is just a single character prefix.
    /// See[LedgerNameSpace enum](https://github.com/ripple/rippled/blob/master/src/ripple/protocol/LedgerFormats.h#L100).
    /// </summary>
    public enum LedgerSpace
    {
        Account = 'a',
        DirNode = 'd',
        GeneratorMap = 'g',
        RippleState = 'r',
        // Entry for an offer.
        Offer = 'o',
        // Directory of things owned by an account.
        OwnerDir = 'O',
        // Directory of order books.
        BookDir = 'B',
        Contract = 'c',
        SkipList = 's',
        Escrow = 'u',
        Amendment = 'f',
        FeeSettings = 'e',
        Ticket = 'T',
        SignerList = 'S',
        Paychan = 'x',
        Check = 'C',
        DepositPreauth = 'p',
    }
}

