using Ripple.Binary.Codec.Enums;

namespace Ripple.Binary.Codec.Types
{
    public class TransactionType : SerializedEnumItem<ushort>
    {
        public class Enumeration : SerializedEnumeration<TransactionType, ushort> { }
        public static Enumeration Values = new Enumeration();
        private TransactionType(string reference, int ordinal) : base(reference, ordinal) { }

        private static TransactionType Add(string name, int ordinal)
        {
            return Values.AddEnum(new TransactionType(name, ordinal));
        }

        // https://github.com/XRPLF/rippled/blob/develop/src/ripple/protocol/TxFormats.h

        /// <summary> invalid transaction </summary>
        public static readonly TransactionType Invalid = Add(nameof(Invalid), -1);
        /// <summary>
        /// This transaction type executes a payment
        /// </summary>
        public static readonly TransactionType Payment = Add(nameof(Payment), 0);
        /// <summary>
        /// This transaction type creates an escrow object.
        /// </summary>
        public static readonly TransactionType EscrowCreate = Add(nameof(EscrowCreate), 1);
        /// <summary>This transaction type completes an existing escrow.  </summary>
        public static readonly TransactionType EscrowFinish = Add(nameof(EscrowFinish), 2);
        /// <summary>This transaction type adjusts various account settings.  </summary>
        public static readonly TransactionType AccountSet = Add(nameof(AccountSet), 3);
        /// <summary>This transaction type cancels an existing escrow.  </summary>
        public static readonly TransactionType EscrowCancel = Add(nameof(EscrowCancel), 4);
        /// <summary>This transaction type sets or clears an account's "regular key"  </summary>
        public static readonly TransactionType SetRegularKey = Add(nameof(SetRegularKey), 5);

        // 6  This transaction type is deprecated; it is retained for historical purposes.

        /// <summary>This transaction type creates an offer to trade one asset for another.  </summary>
        public static readonly TransactionType OfferCreate = Add(nameof(OfferCreate), 7);
        /// <summary>This transaction type cancels existing offers to trade one asset for another.   </summary>
        public static readonly TransactionType OfferCancel = Add(nameof(OfferCancel), 8);

        // 9 This transaction type is deprecated; it is retained for historical purposes. 

        /// <summary>This transaction type creates a new set of tickets.  </summary>
        public static readonly TransactionType TicketCreate = Add(nameof(TicketCreate), 10);

        // 11 This identifier was never used, but the slot is reserved for historical purposes. 

        /// <summary> This transaction type modifies the signer list associated with an account.  </summary>
        public static readonly TransactionType SignerListSet = Add(nameof(SignerListSet), 12);
        /// <summary> This transaction type creates a new unidirectional XRP payment channel. </summary>
        public static readonly TransactionType PaymentChannelCreate = Add(nameof(PaymentChannelCreate), 13);
        /// <summary>This transaction type funds an existing unidirectional XRP payment channel.   </summary>
        public static readonly TransactionType PaymentChannelFund = Add(nameof(PaymentChannelFund), 14);
        /// <summary> This transaction type submits a claim against an existing unidirectional payment channel.  </summary>
        public static readonly TransactionType PaymentChannelClaim = Add(nameof(PaymentChannelClaim), 15);
        /// <summary> This transaction type creates a new check. </summary>
        public static readonly TransactionType CheckCreate = Add(nameof(CheckCreate), 16);
        /// <summary> This transaction type cashes an existing check.  </summary>
        public static readonly TransactionType CheckCash = Add(nameof(CheckCash), 17);
        /// <summary> This transaction type cancels an existing check.  </summary>
        public static readonly TransactionType CheckCancel = Add(nameof(CheckCancel), 18);
        /// <summary> This transaction type grants or revokes authorization to transfer funds.  </summary>
        public static readonly TransactionType DepositPreauth = Add(nameof(DepositPreauth), 19);
        /// <summary> This transaction type modifies a trustline between two accounts. </summary>
        public static readonly TransactionType TrustSet = Add(nameof(TrustSet), 20);
        /// <summary> This transaction type deletes an existing account. </summary>
        public static readonly TransactionType AccountDelete = Add(nameof(AccountDelete), 21);
        /// <summary> This transaction type installs a hook.   <br/>NOTE*: maybe_unused</summary>
        public static readonly TransactionType HookSet = Add(nameof(HookSet), 22);
        /// <summary> This transaction mints a new NFT </summary>
        public static readonly TransactionType NFTokenMint = Add(nameof(NFTokenMint), 25);
        /// <summary> This transaction burns (i.e. destroys) an existing NFT. </summary>
        public static readonly TransactionType NFTokenBurn = Add(nameof(NFTokenBurn), 26);
        /// <summary> This transaction creates a new offer to buy or sell an NFT.  </summary>
        public static readonly TransactionType NFTokenCreateOffer = Add(nameof(NFTokenCreateOffer), 27);
        /// <summary>  This transaction cancels an existing offer to buy or sell an existing NFT. </summary>
        public static readonly TransactionType NFTokenCancelOffer = Add(nameof(NFTokenCancelOffer), 28);
        /// <summary> This transaction accepts an existing offer to buy or sell an existing  NFT. </summary>
        public static readonly TransactionType NFTokenAcceptOffer = Add(nameof(NFTokenAcceptOffer), 29);
        // ...
        /// <summary>
        /// This system-generated transaction type is used to update the status of the various amendments. <br/>
        /// For details, see: https://xrpl.org/amendments.html
        /// </summary>
        public static readonly TransactionType EnableAmendment = Add(nameof(EnableAmendment), 100);
        /// <summary>
        /// This system-generated transaction type is used to update the network's fee settings.<br/>
        /// For details, see: https://xrpl.org/fee-voting.html
        /// </summary>
        public static readonly TransactionType SetFee = Add(nameof(SetFee), 101);
        /// <summary>
        /// This system-generated transaction type is used to update the network's negative UNL
        /// For details, see: https://xrpl.org/negative-unl.html
        /// </summary>
        public static readonly TransactionType UNLModify = Add(nameof(UNLModify), 102);
    }
}