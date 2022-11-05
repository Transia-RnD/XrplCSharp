using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xrpl.BinaryCodec.Types;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Utils;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/models/transactions/NFTokenMint.ts

namespace Xrpl.Models.Transaction
{
    /// <summary>
    /// Transaction Flags for an NFTokenMint Transaction.
    /// </summary>
    [Flags]
    public enum NFTokenMintFlags : uint
    {
        /// <summary>
        /// If set, indicates that the minted token may be burned by the issuer even if the issuer does not currently hold the token.<br/>
        /// The current holder of the token may always burn it.
        /// </summary>
        tfBurnable = 1,
        /// <summary>
        /// If set, indicates that the token may only be offered or sold for XRP.
        /// </summary>
        tfOnlyXRP = 2,
        /// <summary>
        /// If set, indicates that the issuer wants a trustline to be automatically created.
        /// </summary>
        tfTrustLine = 4,
        /// <summary>
        /// If set, indicates that this NFT can be transferred.<br/>
        /// This flag has no effect if the token is being transferred from the issuer or to the issuer.
        /// </summary>
        tfTransferable = 8
    }
    /// <inheritdoc cref="INFTokenMint" />
    public class NFTokenMint : TransactionCommon, INFTokenMint
    {
        public NFTokenMint()
        {
            TransactionType = TransactionType.NFTokenMint;
        }

        /// <inheritdoc />
        public new NFTokenMintFlags? Flags { get; set; }

        /// <inheritdoc />
        public uint NFTokenTaxon { get; set; }

        /// <inheritdoc />
        public string Issuer { get; set; }

        /// <inheritdoc />
        public uint? TransferFee { get; set; }

        /// <inheritdoc />
        public string URI { get; set; }
    }

    /// <summary>
    /// The NFTokenMint transaction creates an NFToken object and adds it to the  relevant NFTokenPage object of the minter.<br/>
    /// If the transaction is  successful, the newly minted token will be owned by the minter account  specified by the transaction.
    /// </summary>
    public interface INFTokenMint : ITransactionCommon
    {
        new NFTokenMintFlags? Flags { get; set; }
        /// <summary>
        /// Indicates the taxon associated with this token.<br/>
        /// The taxon is generally a value chosen by the minter of the token and a given taxon may be used for multiple tokens.<br/>
        /// The implementation reserves taxon identifiers greater than or equal to 2147483648 (0x80000000).<br/>
        /// If you have no use for this field, set it to 0.
        /// </summary>
        uint NFTokenTaxon { get; set; }
        /// <summary>
        /// Indicates the account that should be the issuer of this token.<br/>
        /// This value is optional and should only be specified if the account executing the transaction is not the `Issuer` of the `NFToken` object.<br/>
        /// If it is present, the `MintAccount` field in the `AccountRoot` of the `Issuer` field must match the `Account`, otherwise the transaction will fail.
        /// </summary>
        string Issuer { get; set; }
        /// <summary>
        /// Specifies the fee charged by the issuer for secondary sales of the Token, if such sales are allowed.<br/>
        /// Valid values for this field are between 0 and 50000 inclusive, allowing transfer rates between 0.000% and 50.000% in increments of 0.001%.<br/>
        /// This field must NOT be present if the `tfTransferable` flag is not set.
        /// </summary>
        uint? TransferFee { get; set; }
        /// <summary>
        /// URI that points to the data and/or metadata associated with the NFT.<br/>
        /// This field need not be an HTTP or HTTPS URL; it could be an IPFS URI, a magnet link, immediate data encoded as an RFC2379 "data" URL, or even an opaque issuer-specific encoding.<br/>
        /// The URI is NOT checked for validity, but the field is limited to a maximum length of 256 bytes.<br/>
        /// This field must be hex-encoded.<br/>
        /// You can use `convertStringToHex` to convert this field to the proper encoding.
        /// </summary>
        string URI { get; set; }
    }

    /// <inheritdoc cref="INFTokenMint" />
    public class NFTokenMintResponse : TransactionResponseCommon, INFTokenMint
    {
        public new NFTokenMintFlags? Flags { get; set; }

        /// <inheritdoc />
        public uint NFTokenTaxon { get; set; }

        /// <inheritdoc />
        public string Issuer { get; set; }

        /// <inheritdoc />
        public uint? TransferFee { get; set; }

        /// <inheritdoc />
        public string URI { get; set; }
    }

    public partial class Validation
    {
        //https://github.com/XRPLF/xrpl.js/blob/b40a519a0d949679a85bf442be29026b76c63a22/packages/xrpl/src/models/transactions/NFTokenMint.ts#L100
        /// <summary>
        /// Verify the form and type of an NFTokenMint at runtime.
        /// </summary>
        /// <param name="tx"> An NFTokenMint Transaction.</param>
        /// <exception cref="ValidationError">When the NFTokenMint is Malformed.</exception>
        public static async Task ValidateNFTokenMint(Dictionary<string, dynamic> tx)
        {
            await Common.ValidateBaseTransaction(tx);

            if (tx.TryGetValue("Account", out var Account) && tx.TryGetValue("Issuer", out var Issuer) && Account == Issuer)
                throw new ValidationError("NFTokenMint: Issuer must not be equal to Account");

            if (tx.TryGetValue("URI", out var URI) && URI is string { Length: > 0 } uri && !uri.IsHex())
                throw new ValidationError("NFTokenMint: URI must be in hex format"); 
            
            if (!tx.TryGetValue("NFTokenTaxon", out var NFTokenTaxon) || NFTokenTaxon is null)
                throw new ValidationError("NFTokenMint: missing field NFTokenTaxon");

        }
    }

}
