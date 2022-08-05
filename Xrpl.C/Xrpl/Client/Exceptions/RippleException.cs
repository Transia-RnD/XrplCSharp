#nullable enable
using System;

namespace Xrpl.Client.Exceptions
{
    public class RippleException : Exception
    {
        public RippleException() { }

        public RippleException(string message) : base(message) { }
        public RippleException(string message, Exception? InnerException) : base(message, InnerException) { }
    }
}
