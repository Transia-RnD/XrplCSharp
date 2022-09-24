using System;
using System.Collections.Generic;
using System.Text;

namespace Xrpl.AddressCodecLib
{
    public class AddressCodecException : Exception
    {
        public AddressCodecException() { }

        public AddressCodecException(string message) : base(message){ }
    }
}
