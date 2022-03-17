using System;
using System.Collections.Generic;
using System.Text;

namespace Xrpl.Client.Exceptions
{
    public class RippleException : Exception
    {
        public RippleException() { }

        public RippleException(string message) : base(message){ }
    }
}
