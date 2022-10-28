using System;

namespace Xrpl.Client.Exceptions
{
    public class RippleException : Exception
    {
        public RippleException() { }

        public RippleException(string message) : base(message) { }
        public RippleException(string message, Exception? InnerException) : base(message, InnerException) { }
    }

    public class XrplError : Exception
    {
        //public readonly string message;
        //public readonly byte[]? data;

        //public XrplError(byte[]? data, string message = "")
        //{
        //    //this.name = this.constructor.name; 
        //    this.message = message;
        //    this.data = data;
        //}

        public XrplError() { }

        public XrplError(string message) : base(message) { }
        public XrplError(string message, Exception? InnerException) : base(message, InnerException) { }
    }

    /// <summary>
    /// Error thrown when rippled responds with an error.
    /// </summary>
    public class RippledError : XrplError { }
    /// <summary>
    /// Error thrown when xrpl.js cannot specify error type.
    /// </summary>
    public class UnexpectedError : XrplError { }
    /// <summary>
    /// Error thrown when xrpl.js has an error with connection to rippled.
    /// </summary>
    public class ConnectionError : XrplError
    {
        public ConnectionError(string message) : base(message)
        {
        }
    }
    /// <summary>
    /// Error thrown when xrpl.js is not connected to rippled server.
    /// </summary>
    public class NotConnectedError : XrplError {
        public NotConnectedError(string message = null) : base(message)
        {
        }
    }
    /// <summary>
    /// Error thrown when xrpl.js has disconnected from rippled server.
    /// </summary>
    public class DisconnectedError : XrplError
    {
        public DisconnectedError(string message) : base(message)
        {
        }
    }
    /// <summary>
    /// Error thrown when rippled is not initialized.
    /// </summary>
    public class RippledNotInitializedError : XrplError { }
    /// <summary>
    /// Error thrown when xrpl.js times out.
    /// </summary>
    public class TimeoutError : XrplError { }
    /// <summary>
    /// Error thrown when xrpl.js sees a response in the wrong format.
    /// </summary>
    public class ResponseFormatError : XrplError
    {
        public ResponseFormatError(string message) : base(message)
        {
        }
    }
    /// <summary>
    /// Error thrown when xrpl.js sees a malformed transaction.
    /// </summary>
    public class ValidationError : XrplError
    {
        public ValidationError(string message = null) : base(message)
        {
        }
    }
    /// <summary>
    /// Error thrown when a client cannot generate a wallet from the testnet/devnet
    /// faucets, or when the client cannot infer the faucet URL(i.e.when the Client
    /// is connected to mainnet).
    /// </summary>
    public class XRPLFaucetError : XrplError
    {
        public XRPLFaucetError(string message = null) : base(message)
        {
        }
    }
    /// <summary>
    /// Error thrown when xrpl.js cannot retrieve a transaction, ledger, account, etc.
    /// From rippled.
    /// </summary>
    public class NotFoundError : XrplError
    {
        public NotFoundError(string message = "Not Found") : base(message) { }
    }
}