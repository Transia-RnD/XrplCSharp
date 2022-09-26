using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Xrpl.BinaryCodecLib
{
    public class BinaryCodecException : Exception
    {
        public BinaryCodecException() { }

        public BinaryCodecException(string message) : base(message){ }
    }
}
