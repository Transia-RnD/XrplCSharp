using System;

namespace Xrpl.BinaryCodec
{
    public class BinaryCodecException : Exception
    {
        public BinaryCodecException() { }

        public BinaryCodecException(string message) : base(message){ }
    }
}
