using System;

namespace Xrpl.Client.Exceptions
{
    public class RippleException : Exception
    {
        public RippleException() { }

        public RippleException(string message) : base(message) { }
        public RippleException(string message, Exception? InnerException) : base(message, InnerException) { }
    }

    public class XrplException : Exception
    {
        //public readonly string message;
        //public readonly byte[]? data;

        //public XrplException(byte[]? data, string message = "")
        //{
        //    //this.name = this.constructor.name; 
        //    this.message = message;
        //    this.data = data;
        //}

        public XrplException() { }

        public XrplException(string message) : base(message) { }
        public XrplException(string message, Exception? InnerException) : base(message, InnerException) { }
        //public XrplException(string message, dynamic data) : base(message, data) { }
    }

    /// <summary>
    /// Exception thrown when rippled responds with an Exception.
    /// </summary>
    public class RippledException : XrplException {
        public RippledException(string message, dynamic data = null) : base(message)
        {
        }
    }
    /// <summary>
    /// Exception thrown when xrpl.js cannot specify Exception type.
    /// </summary>
    public class UnexpectedException : XrplException { }
    /// <summary>
    /// Exception thrown when xrpl.js has an Exception with connection to rippled.
    /// </summary>
    public class ConnectionException : XrplException
    {
        public ConnectionException(string message) : base(message)
        {
        }
    }
    /// <summary>
    /// Exception thrown when xrpl.js is not connected to rippled server.
    /// </summary>
    public class NotConnectedException : XrplException
    {
        public NotConnectedException(string message = null) : base(message)
        {
        }
    }
    /// <summary>
    /// Exception thrown when xrpl.js has disconnected from rippled server.
    /// </summary>
    public class DisconnectedException : XrplException
    {
        public DisconnectedException(string message) : base(message)
        {
        }
    }
    /// <summary>
    /// Exception thrown when rippled is not initialized.
    /// </summary>
    public class RippledNotInitializedException : XrplException { }
    /// <summary>
    /// Exception thrown when xrpl.js times out.
    /// </summary>
    public class TimeoutException : XrplException { }
    /// <summary>
    /// Exception thrown when xrpl.js sees a response in the wrong format.
    /// </summary>
    public class ResponseFormatException : XrplException
    {
        public ResponseFormatException(string message, dynamic data = null) : base(message)
        {
        }
    }
    /// <summary>
    /// Exception thrown when xrpl.js sees a malformed transaction.
    /// </summary>
    public class ValidationException : XrplException
    {
        public ValidationException(string message = null) : base(message)
        {
        }
    }
    /// <summary>
    /// Exception thrown when a client cannot generate a wallet from the testnet/devnet
    /// faucets, or when the client cannot infer the faucet URL(i.e.when the Client
    /// is connected to mainnet).
    /// </summary>
    public class XRPLFaucetException : XrplException
    {
        public XRPLFaucetException(string message = null) : base(message)
        {
        }
    }
    /// <summary>
    /// Exception thrown when xrpl.js cannot retrieve a transaction, ledger, account, etc.
    /// From rippled.
    /// </summary>
    public class NotFoundException : XrplException
    {
        public NotFoundException(string message = "Not Found") : base(message) { }
    }
}