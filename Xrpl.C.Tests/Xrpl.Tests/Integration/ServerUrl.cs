using System;
using System.Diagnostics;

namespace Xrpl.Tests.Client.Tests.Integration
{
    public class ServerUrl
    {
        static string HOST = "localhost";
        static string PORT = "6006";
        public static string serverUrl = $"ws://{HOST}:{PORT}";
    }
}