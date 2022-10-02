using System;

namespace Xrpl.BinaryCodec.Types
{
    /// <summary>
    /// Thrown when JSON is not valid.
    /// </summary>
    public class InvalidJsonException : Exception
    {
        /// <inheritdoc />
        public InvalidJsonException() 
        {
        }

        /// <inheritdoc />
        public InvalidJsonException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public InvalidJsonException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}