using System;
namespace Xrpl.Models.Common
{
	public class NFTokenIDData
	{
        public NFTokenIDData(string nftokenId, UInt32 flags, UInt32 transferFee, string issuer, UInt32 taxon, UInt32 sequence)
        {
            NFTokenID = nftokenId;
            Flags = flags;
            TransferFee = transferFee;
            Issuer = issuer;
            Taxon = taxon;
            Sequence = sequence;
        }

        public string NFTokenID { get; set; }

        public UInt32 Flags { get; set; }

        public UInt32 TransferFee { get; set; }

        public string Issuer { get; set; }

        public UInt32 Taxon { get; set; }

        public UInt32 Sequence { get; set; }
    }
}

