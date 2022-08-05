using System;
using Org.BouncyCastle.Crypto.Digests;

namespace Ripple.Keypairs.Utils
{
    public class Sha512
    {
        private readonly Sha512Digest _digest;

        public Sha512() => _digest = new Sha512Digest();

        public Sha512(byte[] start) : this() => Add(start);

        public Sha512 Add(byte[] bytes)
        {
            _digest.BlockUpdate(bytes, 0, bytes.Length);
            return this;
        }

        public Sha512 AddU32(uint i)
        {
            _digest.Update((byte)(i >> 24 & 0xFFu));
            _digest.Update((byte)(i >> 16 & 0xFFu));
            _digest.Update((byte)(i >> 8 & 0xFFu));
            _digest.Update((byte)(i & 0xFFu));
            return this;
        }

        private byte[] FinishTaking(int size)
        {
            var finished = Finish();

            var hash = new byte[size];
            Array.Copy(finished, 0, hash, 0, size);
            return hash;
        }

        public byte[] Finish()
        {
            var finished = new byte[64];
            _digest.DoFinal(finished, 0);
            return finished;
        }

        public byte[] Finish128() => FinishTaking(16);

        public byte[] Finish256() => FinishTaking(32);

        public static byte[] Half(byte[] input) => new Sha512(input).Finish256();

        public static byte[] Quarter(byte[] input) => new Sha512(input).Finish128();
    }
}