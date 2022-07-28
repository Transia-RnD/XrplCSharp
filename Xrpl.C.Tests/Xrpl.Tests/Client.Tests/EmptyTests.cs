using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Types;
using Xrpl.Client.Model;
using Xrpl.Client.Model.Account;
using Xrpl.Client.Requests.Account;
using System;
using System.Diagnostics;

namespace Xrpl.Client.Tests
{
    [TestClass]
    public class EmptyTests
    {
        [TestMethod]
        public async Task CanGetAccountOffersVerify()
        {
            try
            {
                string serverUrl = "wss://xrplcluster.com";
                IRippleClient client1 = new RippleClient(serverUrl);
                client1.Connect();
                AccountOffers accountOffers = await client1.AccountOffers("rLiooJRSKeiNfRJcDBUhu4rcjQjGLWqa4p");
                Debug.WriteLine(accountOffers.Offers.Count);
                Assert.IsNotNull(accountOffers);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }


    }
}
