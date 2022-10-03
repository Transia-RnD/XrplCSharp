using System;

namespace Xrpl.BinaryCodec
{
    public class BinaryCodecError : Exception
    {
        public BinaryCodecError() { }

        public BinaryCodecError(string message) : base(message){ }
    }
}
