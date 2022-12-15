using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xrpl.BinaryCodec.Serdes;
using Xrpl.BinaryCodec.Types;



namespace Xrpl.BinaryCodec
{
    public class Sha512Half : BytesList
    {
        private readonly SHA512 _hash = SHA512.Create();

        public static Sha512Half put(byte[] bytes)
        {
            return new Sha512Half().Put(bytes);
        }

        public new Sha512Half Put(byte[] bytes)
        {
            _hash.TransformBlock(bytes, 0, bytes.Length, null, 0);
            return this;
        }

        public byte[] Finish256()
        {
            _hash.TransformFinalBlock(new byte[0], 0, 0);
            var digest = _hash.Hash;
            return digest.Take(32).ToArray();
        }

        public Hash256 Finish()
        {
            return new Hash256(Finish256());
        }
    }

    public static class Sha512HalfExtensions
    {
        public static byte[] Sha512Half(this IEnumerable<byte[]> args)
        {
            var hash = new Sha512Half();
            foreach (var a in args)
            {
                hash.Put(a);
            }
            return hash.Finish256();
        }
    }

    public static class Transaction
    {
        public static Hash256 Id(byte[] serialized)
        {
            return new Hash256(new[] { HashPrefix.TransactionID, serialized}.Sha512Half());
        }
    }
}