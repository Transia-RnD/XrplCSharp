using System;
using System.Collections.Generic;
using System.Text;

namespace Xrpl.Client.Exceptions
{
    public class RippleException : Exception
    {
        public RippleException() { }

        public RippleException(string message) : base(message) {}
        #pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public RippleException(string message, Exception? InnerException) : base(message, InnerException) {}
    }
}
