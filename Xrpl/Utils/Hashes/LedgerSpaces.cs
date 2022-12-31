

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/hashes/ledgerSpaces.ts

namespace Xrpl.Utils.Hashes
{
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

