using System;
using System.Security.Cryptography;
using Ripple.Binary.Codec.Binary;

namespace Ripple.Binary.Codec.Hashing
{
    public class Sha512 : IBytesSink
    {
        private readonly SHA512 _digest;

        public Sha512()
        {
            _digest = SHA512.Create();
            _digest.Initialize();
        }

        public Sha512(byte[] start) : this() => Add(start);

        public Sha512(uint prefix) : this() => AddU32(prefix);

        public Sha512 Add(byte[] bytes)
        {
            _digest.TransformBlock(bytes, 0, bytes.Length, null, 0);
            return this;
        }

        public Sha512 Add(byte one) => Add(new[] {one});

        public Sha512 AddU32(uint i)
        {
            Add(new[]
            {
                (byte) (i >> 24),
                (byte) (i >> 16),
                (byte) (i >> 8),
                (byte) i
            });
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
            _digest.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            var hash =_digest.Hash;
            _digest.Dispose();
            return hash;
        }

        public byte[] Finish128() => FinishTaking(16);

        public byte[] Finish256() => FinishTaking(32);

        public static byte[] Half(byte[] input, uint? prefix = null)
        {
            var hasher = new Sha512();
            if (prefix != null)
            {
                hasher.AddU32((uint) prefix);
            }
            hasher.Add(input);
            return hasher.Finish256();
        }
        public static byte[] Quarter(byte[] input) => new Sha512(input).Finish128();

        public void Put(byte aByte) => Add(aByte);

        public void Put(byte[] bytes) => Add(bytes);
    }
}
