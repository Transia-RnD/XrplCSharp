using System;

namespace Xrpl.BinaryCodecLib
{
    public class BinaryCodecException : Exception
    {
        public BinaryCodecException() { }

        public BinaryCodecException(string message) : base(message){ }
    }
}
