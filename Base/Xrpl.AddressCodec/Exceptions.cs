using System;

namespace Xrpl.AddressCodecLib
{
    public class AddressCodecException : Exception
    {
        public AddressCodecException() { }

        public AddressCodecException(string message) : base(message){ }
    }
}
