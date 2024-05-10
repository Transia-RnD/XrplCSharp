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

        public uint Flags { get; set; }

        public uint TransferFee { get; set; }

        public string Issuer { get; set; }

        public uint Taxon { get; set; }

        public uint Sequence { get; set; }
    }
}

