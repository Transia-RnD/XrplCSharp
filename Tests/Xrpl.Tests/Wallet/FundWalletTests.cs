using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client;
using Xrpl.Client.Exceptions;
using Xrpl.Wallet;

namespace Xrpl.Tests.Wallet.Tests
{
    [TestClass]
    public class FundWalletTests
    {
        [TestMethod]
        public async Task TestUFaucetHostsAsync()
        {
            string serverUrl = "wss://s.altnet.rippletest.net:51233";
            XrplClient client = new XrplClient(serverUrl);
            client.Connect();
            XrplWallet wallet = XrplWallet.Generate();
            await WalletSugar.FundWallet(client, wallet);
        }
    }

    [TestClass]
    public class TestTimer
    {

        private static int attempts = 1;
        private static double finalBalance;
        private static System.Timers.Timer aTimer;

        private static double _originalBalance;
        private static string _address;
        private static XrplClient _client;

        private static async void OnTimedEventAsync(Object source, ElapsedEventArgs e)
        {
            // This piece of code will run after every 1000 ms
            if (attempts < 0)
            {
                finalBalance = _originalBalance;
                aTimer.Enabled = false;
            }
            else
            {
                attempts -= 1;
            }
            try
            {
                double newBalance = 0;
                try
                {
                    newBalance = Convert.ToDouble(await _client.GetXrpBalance(_address));
                }
                catch (RippleException err)
                {
                    Console.WriteLine(err);
                    /* newBalance remains undefined */
                }
                if (newBalance > _originalBalance)
                {
                    finalBalance = newBalance;
                    aTimer.Enabled = false;
                }
            }
            catch (InvalidCastException err)
            {
                aTimer.Enabled = false;
                if (err is RippledError)
                {
                    throw new XRPLFaucetError($"Unable to check if the address {_address} balance has increased.Error: {"err.message"}");
                }
                throw new XRPLFaucetError($"Unable to check if the address {_address} balance has increased.Error: {"err.message"}");
            }
        }

        public static async Task<double> GetUpdatedBalance(
            XrplClient client,
            string address,
            double originalBalance
        )
        {
            _client = client;
            _address = address;
            _originalBalance = originalBalance;
            aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += (sender, e) => OnTimedEventAsync(sender, e);
            aTimer.Enabled = true;
            aTimer.Start();
            while (aTimer.Enabled)
            {
                Task.Delay(1000).Wait();
            }
            aTimer.Stop();
            return finalBalance;

        }

        [TestMethod]
        public async Task TestUTimer()
        {
            string serverUrl = "wss://s.altnet.rippletest.net:51233";
            XrplClient client = new XrplClient(serverUrl);
            client.Connect();
            XrplWallet wallet = XrplWallet.Generate();
            await GetUpdatedBalance(client, wallet.ClassicAddress, 0);
        }
    }
}

