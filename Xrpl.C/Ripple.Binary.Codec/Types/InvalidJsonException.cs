using System;

namespace Ripple.Binary.Codec.Types
{
    /// <summary>
    /// Thrown when JSON is not valid.
    /// </summary>
    public class InvalidJsonException : Exception
    {
        public InvalidJsonException() 
        {
        }

        public InvalidJsonException(string message) : base(message)
        {
        }

        public InvalidJsonException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}