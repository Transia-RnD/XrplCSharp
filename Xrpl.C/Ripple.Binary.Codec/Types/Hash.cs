using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Util;

namespace Ripple.Binary.Codec.Types
{
    public abstract class Hash : ISerializedType, IEquatable<Hash>
    {
        public readonly byte[] Buffer;
        protected Hash(byte[] buffer)
        {
            Buffer = buffer;
        }

        public void ToBytes(IBytesSink sink)
        {
            sink.Put(Buffer);
        }

        public JToken ToJson()
        {
            return ToString();
        }

        public bool Equals(Hash other)
        {
            return other.Buffer.SequenceEqual(Buffer);
        }

        public override string ToString()
        {
            return B16.Encode(Buffer);
        }

        public static explicit operator string(Hash h) => h.ToHex();
    }
}