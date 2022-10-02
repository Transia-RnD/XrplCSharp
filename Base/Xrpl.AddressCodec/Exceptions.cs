using System;

namespace Xrpl.AddressCodec
{
    public class AddressCodecException : Exception
    {
        public AddressCodecException() { }

        public AddressCodecException(string message) : base(message){ }
    }
}
