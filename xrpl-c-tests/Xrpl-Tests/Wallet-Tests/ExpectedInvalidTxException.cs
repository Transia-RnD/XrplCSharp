using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ripple.TxSigning.Tests
{
    /// <summary>
    /// Indicates that an <see cref="InvalidTxException"/> with provided message text is expected during test method execution.
    /// </summary>
    /// <remarks>
    /// Expected exception message defaults to 'Transaction is not valid.\r\nParameter name: tx'.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ExpectedInvalidTxException : ExpectedExceptionBaseAttribute
    {
        public string ExpectedExceptionMessage { get; private set; }
        public string WrongExceptionTypeMessage { get; private set; }

        public ExpectedInvalidTxException() : this("Transaction is not valid.\r\nParameter name: tx", "No exception was thrown.")
        {
        }

        public ExpectedInvalidTxException(string expectedExceptionMessage) : this(expectedExceptionMessage, "No exception was thrown.")
        {
        }

        public ExpectedInvalidTxException(string expectedExceptionMessage, string noExceptionMessage) : base(noExceptionMessage)
        {
            ExpectedExceptionMessage = expectedExceptionMessage;
            WrongExceptionTypeMessage = "The exception that was thrown does not derive from InvalidTxException.";
        }

        protected override void Verify(Exception exception)
        {
            Assert.IsNotNull(exception);

            base.RethrowIfAssertException(exception);

            Assert.IsInstanceOfType(exception, typeof(InvalidTxException), WrongExceptionTypeMessage);

            if(ExpectedExceptionMessage != null)
                Assert.AreEqual(ExpectedExceptionMessage, exception.Message, "Wrong exception message.");
        }
    }
}
