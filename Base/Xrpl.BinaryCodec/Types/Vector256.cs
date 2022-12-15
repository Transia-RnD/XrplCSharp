using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Xrpl.BinaryCodec.Serdes;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/ripple-binary-codec/src/types/vector-256.ts

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Class for serializing and deserializing vectors of Hash256
    /// </summary>
    public class Vector256 : SerializedType
    {
        public Vector256(byte[] bytes)
        {
            Bytes = bytes;
        }

        public byte[] Bytes { get; }

        /// <summary>
        /// TypeGuard for Array<string>
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private static bool IsStrings(object arg)
        {
            return arg is string[] && (arg as string[]).Length == 0; // todo: fix see original
        }

        /// <summary>
        /// Construct a Vector256 from a BinaryParser
        /// </summary>
        /// <param name="parser">BinaryParser to</param>
        /// <param name="hint">length of the vector, in bytes, optional</param>
        /// <returns>a Vector256 object</returns>
        public static Vector256 FromParser(BinaryParser parser, int? hint = null)
        {
            var bytesList = new BytesList();
            var bytes = hint ?? parser.Size();
            var hashes = bytes / 32;
            for (var i = 0; i < hashes; i++)
            {
                Hash256.FromParser(parser).ToBytesSink(bytesList);
            }
            return new Vector256(bytesList.ToBytes());
        }

        /// <summary>
        /// Construct a Vector256 object from an array of hashes
        /// </summary>
        /// <param name="value">A Vector256 object or array of hex-strings representing Hash256's</param>
        /// <returns>a Vector256 object</returns>
        public static Vector256 From(object value)
        {
            if (value is Vector256)
            {
                return (Vector256)value;
            }

            if (IsStrings(value))
            {
                var bytesList = new BytesList();
                foreach (var hash in value as string[])
                {
                    Hash256.From(hash).ToBytesSink(bytesList);
                }
                return new Vector256(bytesList.ToBytes());
            }

            throw new Exception("Cannot construct Vector256 from given value");
        }

        /// <summary>
        /// Return an Array of hex-strings represented by this.bytes
        /// </summary>
        /// <returns>An Array of strings representing the Hash256 objects</returns>
        public string[] ToJson()
        {
            if (this.Bytes.Length % 32 != 0)
            {
                throw new Exception("Invalid bytes for Vector256");
            }

            var result = new List<string>();
            for (var i = 0; i < this.Bytes.Length; i += 32)
            {
                result.Add(this.Bytes.Skip(i).Take(32).ToArray().ToHex().ToUpper());
            }
            return result.ToArray();
        }
    }
}