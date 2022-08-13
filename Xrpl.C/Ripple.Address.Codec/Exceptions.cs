using System;
using System.Collections.Generic;
using System.Text;

namespace Ripple.Address.Codec.Exceptions
{
    public class AddressCodecException : Exception
    {
        public AddressCodecException() { }

        public AddressCodecException(string message) : base(message){ }
    }
}
