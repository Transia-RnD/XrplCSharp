using System;

namespace Xrpl.AddressCodec
{
    public class KeypairException : Exception
    {
        public KeypairException() { }

        public KeypairException(string message) : base(message){ }
    }
}
