using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Xrpl.BinaryCodec.Binary;
using Xrpl.BinaryCodec.Util;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/hash.ts

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// The XRP Ledger has several "hash" types: Hash128, Hash160, and Hash256.<br/>
    /// These fields contain arbitrary binary data of the given number of bits, which may or may not represent the result of a hash operation.<br/>
    /// All such fields are serialized as the specific number of bits, with no length indicator, in big-endian byte order.<br/>
    ///
    /// Base class defining how to encode and decode hashes
    /// </summary>
    public abstract class Hash : ISerializedType, IEquatable<Hash>
    {
        /// <summary> Bytes buffer </summary>
        public readonly byte[] Buffer;
        /// <summary>
        /// Defines how to construct Hash from buffer 
        /// </summary>
        /// <param name="buffer">bytes buffer</param>
        protected Hash(byte[] buffer)
        {
            Buffer = buffer;
        }

        /// <summary> Hash to bytes </summary>
        /// <param name="sink"> Bytes Sink </param>
        public void ToBytes(IBytesSink sink)
        {
            sink.Put(Buffer);
        }
        /// <summary> hash to json string </summary>
        public JToken ToJson() => ToString();

        /// <summary>
        /// check hash to equal
        /// </summary>
        /// <param name="other">other hash</param>
        /// <returns></returns>
        public bool Equals(Hash other) => other is not null && other.Buffer.SequenceEqual(Buffer);
        /// <summary> hash to string value </summary>
        /// <returns></returns>
        public override string ToString() => B16.Encode(Buffer);

        public static explicit operator string(Hash h) => h.ToHex();
    }
}