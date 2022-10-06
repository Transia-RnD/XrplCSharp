using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Xrpl.AddressCodec;
using Xrpl.Client;
using Xrpl.Client.Exceptions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/Wallet/fundWallet.ts

namespace Xrpl.Wallet
{
    public static class EasyTimer
    {
        public static IDisposable SetInterval(Action method, int delayInMilliseconds)
        {
            System.Timers.Timer timer = new System.Timers.Timer(delayInMilliseconds);
            timer.Elapsed += (source, e) =>
            {
                method();
            };

            timer.Enabled = true;
            timer.Start();

            // Returns a stop handle which can be used for stopping
            // the timer, if required
            return timer as IDisposable;
        }

        public static IDisposable SetTimeout(Action method, int delayInMilliseconds)
        {
            System.Timers.Timer timer = new System.Timers.Timer(delayInMilliseconds);
            timer.Elapsed += (source, e) =>
            {
                method();
            };

            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Start();

            // Returns a stop handle which can be used for stopping
            // the timer, if required
            return timer as IDisposable;
        }
    }

    public class WalletSugar
    {
        //Interval to check an account balance
        static int INTERVAL_SECONDS = 1;
        //Maximum attempts to retrieve a balance
        static int MAX_ATTEMPTS = 20;

        public class Funded
        {
            public XrplWallet Wallet;
            public double Balance;

            public Funded(XrplWallet wallet, double balance)
            {
                Wallet = wallet;
                Balance = balance;
            }
        }

        public class FaucetAccount
        {
            [JsonProperty("xAddress")]
            public string XAddress { get; set; }

            [JsonProperty("classicAddress")]
            public string ClassicAddress { get; set; }

            [JsonProperty("secret")]
            public string Secret { get; set; }

        }

        public class FaucetWallet
        {
            [JsonProperty("account")]
            public FaucetAccount Account { get; set; }

            [JsonProperty("amount")]
            public double Amount { get; set; }

            [JsonProperty("balance")]
            public double Balance { get; set; }

        }

        public static class FaucetNetwork
        {
            public static readonly string Testnet = "faucet.altnet.rippletest.net";
            public static readonly string Devnet = "faucet.devnet.rippletest.net";
            public static readonly string NFTDevnet = "faucet-nft.ripple.com";
        }

        public static async Task<Funded> FundWallet(XrplClient client, XrplWallet? wallet = null, string? faucetHost = null)
        {
            //if (!client.IsConnected())
            //{
            //    throw new RippledError("Client not connected, cannot call faucet");
            //}
            // Generate a new Wallet if no existing Wallet is provided or its address is invalid to fund
            XrplWallet walletToFund = (wallet != null && XrplCodec.IsValidClassicAddress(wallet.ClassicAddress)) ? wallet : XrplWallet.Generate();

            double startingBalance = 0;
            try
            {
                startingBalance = Convert.ToDouble(await client.GetXrpBalance(walletToFund.ClassicAddress));
            }
            catch
            {
                /* startingBalance remains '0' */
            }

            // Create the POST request body

            Dictionary<string, dynamic> json = new Dictionary<string, dynamic>
            {
                { "destination", walletToFund.ClassicAddress },
            };
            string jsonData = JsonConvert.SerializeObject(json);
            byte[] postBody = Encoding.UTF8.GetBytes(jsonData);
            Dictionary<string, dynamic> httpOptions = GetHTTPOptions(client, postBody, faucetHost);
            return await ReturnPromise(httpOptions, client, startingBalance, walletToFund, jsonData);
        }

        public static async Task<Funded> ReturnPromise(
              Dictionary<string, dynamic> options,
              XrplClient client,
              double startingBalance,
              XrplWallet walletToFund,
              string postBody
        )
        {
            HttpClient httpsClient = new HttpClient();
            httpsClient.BaseAddress = new Uri($"https://{options["hostname"]}");
            httpsClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            StringContent contentData = new StringContent(postBody, Encoding.UTF8, "application/json");
            var response = await httpsClient.PostAsync(options["path"], contentData);
            HttpContent content = response.Content;
            byte[] chunks = await content.ReadAsByteArrayAsync();
            return await OnEnd(
                response,
                chunks,
                client,
                startingBalance,
                walletToFund
            );
        }

        public static Dictionary<string, dynamic> GetHTTPOptions(
              XrplClient client,
              byte[] postBody,
              string hostname
        )
        {
            Dictionary<string, dynamic> options = new Dictionary<string, dynamic>
            {
                { "hostname", hostname != null ? hostname : GetFaucetHost(client) },
                { "port", 443 },
                { "path", "/accounts" },
                { "method", "POST" },
                { "headers", new Dictionary<string, dynamic> {
                    { "Content-Type", "application/json" },
                    { "Content-Length", postBody.Length }
                } }
            };
            return options;
        }
        public static async Task<Funded> OnEnd(
            HttpResponseMessage response,
            byte[] chunks,
            XrplClient client,
            double startingBalance,
            XrplWallet walletToFund
        )
        {
            // Get Content Headers
            string body = Encoding.UTF8.GetString(chunks);
            // "application/json; charset=utf-8"
            if (response.Content.Headers.GetValues("Content-Type").First().StartsWith("application/json"))
            {
                return await ProcessSuccessfulResponse(
                    client,
                    body,
                    startingBalance,
                    walletToFund
                );
            }
            else
            {
                Dictionary<string, dynamic> errorResponse = new Dictionary<string, dynamic>
                {
                    { "statusCode", response.StatusCode },
                    { "contentType", response.Content.Headers.GetValues("Content-Type").First() },
                    { "body", body },
                };
                return await Task.FromException<Funded>(new XRPLFaucetError($"Content type is not application json {errorResponse.ToString()}"));
            }
        }

        public static async Task<Funded> ProcessSuccessfulResponse(
              XrplClient client,
              string body,
              double startingBalance,
              XrplWallet walletToFund
        )
        {
            FaucetWallet faucetWallet = JsonConvert.DeserializeObject<FaucetWallet>(body);
            string classicAddress = faucetWallet.Account.ClassicAddress;
            if (classicAddress == null)
            {
                return await Task.FromException<Funded>(new XRPLFaucetError("The faucet account is undefined"));
            }
            try
            {
                // Check at regular interval if the address is enabled on the XRPL and funded
                double updatedBalance = await GetUpdatedBalance(
                  client,
                  classicAddress,
                  startingBalance
                );
                if (updatedBalance > startingBalance)
                {
                    Funded funded = new Funded(
                        walletToFund,
                        Convert.ToDouble(
                            await GetUpdatedBalance(
                                client,
                                walletToFund.ClassicAddress,
                                startingBalance
                            )
                        )
                    );
                    return funded;
                }
                else
                {
                    throw new XRPLFaucetError($"Unable to fund address with faucet after waiting {INTERVAL_SECONDS} * {MAX_ATTEMPTS} seconds");
                }
            }
            catch (Exception err)
            {
                if (err is Exception)
                {
                    return await Task.FromException<Funded>(new XRPLFaucetError(err.Message));
                }
                return await Task.FromException<Funded>(err);
            }
        }

        private static int attempts = MAX_ATTEMPTS;
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

        public static string GetFaucetHost(XrplClient client)
        {
            string connectionUrl = client.url;
            // 'altnet' for Ripple Testnet server and 'testnet' for XRPL Labs Testnet server
            if (connectionUrl.Contains("altnet") || connectionUrl.Contains("testnet"))
            {
            return FaucetNetwork.Testnet;
            }

            if (connectionUrl.Contains("devnet"))
            {
                return FaucetNetwork.Devnet;
            }

            if (connectionUrl.Contains("xls20-sandbox"))
            {
                return FaucetNetwork.NFTDevnet;
            }

            throw new XRPLFaucetError("Faucet URL is not defined or inferrable.");
        }
    }
}