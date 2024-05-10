using System;

namespace Xrpl.BinaryCodec
{
    public class BinaryCodecException : Exception
    {
        public BinaryCodecException() { }

        public BinaryCodecException(string message) : base(message){ }
    }
    
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
