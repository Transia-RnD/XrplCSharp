using System;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Binary;

//https://github.com/XRPLF/xrpl.js/blob/8a9a9bcc28ace65cde46eed5010eb8927374a736/packages/ripple-binary-codec/src/types/uint.ts

namespace Ripple.Binary.Codec.Types
{
    /// <summary>
    /// Base class for serializing and deserializing unsigned integers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Uint<T> : ISerializedType where T: struct, IConvertible
    {
        /// <summary>
        /// integers value
        /// </summary>
        public readonly T Value;
        /// <summary>
        /// create instance of this integer value
        /// </summary>
        /// <param name="value"></param>
        protected Uint(T value)
        {
            Value = value;
        }

        /// <inheritdoc />
        public void ToBytes(IBytesSink sink) => sink.Put(ToBytes());

        /// <inheritdoc />
        public virtual JToken ToJson() => Convert.ToUInt32(Value);

        /// <inheritdoc />
        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// convert to bytes array
        /// </summary>
        public abstract byte[] ToBytes();
    }
}