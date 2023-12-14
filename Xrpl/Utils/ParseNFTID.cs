using System;
using Xrpl.AddressCodec;
using Xrpl.BinaryCodec.Util;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Common;
using Xrpl.BinaryCodec.Hashing;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/parseNFTokenID.ts

namespace Xrpl.Utils
{
    public static class ParseNFTID
    {
        // Reference to the xrpl.js library
        // https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/utils/parseNFTokenID.ts

        /// <summary>
        /// An issuer may issue several NFTs with the same taxon; to ensure that NFTs are
        ///
        /// spread across multiple pages we lightly mix the taxon up by using the sequence
        /// (which is not under the issuer's direct control) as the seed for a simple linear
        /// congruential generator.
        ///
        /// From the Hull-Dobell theorem we know that f(x)=(m* x+c) mod n will yield a
        /// permutation of[0, n) when n is a power of 2 if m is congruent to 1 mod 4 and
        /// c is odd.By doing a bitwise XOR with this permutation we can scramble / unscramble
        /// the taxon.
        ///
        /// The XLS - 20d proposal fixes m = 384160001 and c = 2459.
        /// We then take the modulus of 2 ^ 32 which is 4294967296.
        /// </summary>
        /// <param name="taxon"> The scrambled or unscrambled taxon (The XOR is both the encoding and decoding). </param>
        /// <param name="tokenSeq"> The account sequence when the token was minted. Used as a psuedorandom seed. </param>
        /// <returns> The opposite taxon. If the taxon was scrambled it becomes unscrambled, and vice versa.</returns>
        public static uint UnscrambleTaxon(uint taxon, uint tokenSeq)
        {
            return (uint)((taxon ^ (384160001 * tokenSeq + 2459)) % 4294967296);
        }

        public static NFTokenIDData ParseNFTokenID(this string nftokenID)
        {
            const int expectedLength = 64;
            if (nftokenID.Length != expectedLength)
            {
                throw new XrplException($"Attempting to parse a nftokenID with length ${nftokenID.Length}\n" +
                                        $", but expected a token with length ${expectedLength}");
            }

            uint flags = Convert.ToUInt32(nftokenID.Substring(0, 4), 16);
            uint transferFee = Convert.ToUInt32(nftokenID.Substring(4, 4), 16);
            string scrambledTaxon = nftokenID.Substring(48, 8);
            uint sequence = Convert.ToUInt32(nftokenID.Substring(56, 8), 16);
            uint taxon = UnscrambleTaxon(Convert.ToUInt32(scrambledTaxon, 16), sequence);

            string issuer = XrplCodec.EncodeAccountID(nftokenID.Substring(8, 40).FromHexToBytes());
                        
            return new NFTokenIDData(nftokenID, flags, transferFee, issuer, taxon, sequence);
        }

        public static string ParseNFTOffer(string account, uint sequence)
        {

            byte[] PREFIX = { 0x00, 0x71 };
            byte[] accountBytes = XrplCodec.DecodeAccountID(account);
            byte[] sequenceBytes = Bits.GetBytes(sequence);

            byte[] rv = new byte[PREFIX.Length + accountBytes.Length + sequenceBytes.Length];
            System.Buffer.BlockCopy(PREFIX, 0, rv, 0, PREFIX.Length);
            System.Buffer.BlockCopy(accountBytes, 0, rv, PREFIX.Length, accountBytes.Length);
            System.Buffer.BlockCopy(sequenceBytes, 0, rv, PREFIX.Length + accountBytes.Length, sequenceBytes.Length);
            byte[] response = Sha512.Half(rv);
            return response.ToHex();
        }

        public static string ParseNFTokenOfferOwner(this string nftokenOfferId)
        {
            const int expectedLength = 64;
            if (nftokenOfferId.Length != expectedLength)
            {
                throw new XrplException($"Attempting to parse a nftokenID with length ${nftokenOfferId.Length}\n" +
                                        $", but expected a token with length ${expectedLength}");
            }
            string issuer = XrplCodec.EncodeAccountID(nftokenOfferId.Substring(8, 40).FromHexToBytes());
            return issuer;
        }

    }
}
