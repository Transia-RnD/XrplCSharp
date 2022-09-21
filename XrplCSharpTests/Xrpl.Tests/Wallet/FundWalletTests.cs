using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client;
using Xrpl.Client.Exceptions;
using Xrpl.Tests.Client.Tests.Integration;
using Xrpl.XrplWallet;

namespace Xrpl.Tests.XrplWallet.Tests
{
    [TestClass]
    public class FundWalletTests
    {
        [TestMethod]
        public async Task TestUFaucetHostsAsync()
        {
            string serverUrl = "wss://s.altnet.rippletest.net:51233";
            RippleClient client = new RippleClient(serverUrl);
            client.Connect();
            Wallet wallet = Wallet.Generate();
            Debug.WriteLine(wallet);
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
        private static RippleClient _client;

        private static async void OnTimedEventAsync(Object source, ElapsedEventArgs e)
        {
            Debug.WriteLine("OnTimedEventAsync");
            Debug.WriteLine($"ATTEMPTS: {attempts}");
            // This piece of code will run after every 1000 ms
            if (attempts < 0)
            {
                Debug.WriteLine("1.1");
                finalBalance = _originalBalance;
                aTimer.Enabled = false;
            }
            else
            {
                Debug.WriteLine("1.2");
                attempts -= 1;
            }
            try
            {
                Debug.WriteLine("2");
                double newBalance = 0;
                try
                {
                    Debug.WriteLine("2.1");
                    //Task.Delay(2000).Wait();
                    //newBalance = 1000.0;
                    newBalance = Convert.ToDouble(await _client.GetXrpBalance(_address));
                    Debug.WriteLine("NEW BALANCE");
                }
                catch (RippleException err)
                {
                    Debug.WriteLine(err);
                    Debug.WriteLine("2.2");
                    /* newBalance remains undefined */
                }
                Debug.WriteLine("3");
                Debug.WriteLine(newBalance);
                Debug.WriteLine(_originalBalance);
                if (newBalance > _originalBalance)
                {
                    Debug.WriteLine("3.1");
                    finalBalance = newBalance;
                    aTimer.Enabled = false;
                }
            }
            catch (InvalidCastException err)
            {
                Debug.WriteLine(err);
                Debug.WriteLine("-1");
                aTimer.Enabled = false;
                if (err is RippledError)
                {
                    throw new XRPLFaucetError($"Unable to check if the address {_address} balance has increased.Error: {"err.message"}");
                }
                throw new XRPLFaucetError($"Unable to check if the address {_address} balance has increased.Error: {"err.message"}");
            }
        }

        public static async Task<double> GetUpdatedBalance(
            RippleClient client,
            string address,
            double originalBalance
        )
        {
            Debug.WriteLine("GetUpdatedBalance");
            Debug.WriteLine("0");
            Debug.WriteLine(finalBalance);
            _client = client;
            _address = address;
            _originalBalance = originalBalance;
            aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += (sender, e) => OnTimedEventAsync(sender, e);
            aTimer.Enabled = true;
            aTimer.Start();
            Debug.WriteLine("TIMER STARTED");
            while (aTimer.Enabled)
            {
                Task.Delay(1000).Wait();
            }
            aTimer.Stop();
            Debug.WriteLine("TIMER FINISHED");
            Debug.WriteLine(finalBalance);
            return finalBalance;

        }

        [TestMethod]
        public async Task TestUTimer()
        {
            string serverUrl = "wss://s.altnet.rippletest.net:51233";
            RippleClient client = new RippleClient(serverUrl);
            client.Connect();
            Wallet wallet = Wallet.Generate();
            Debug.WriteLine(wallet);
            await GetUpdatedBalance(client, wallet.ClassicAddress, 0);
        }
    }
}

