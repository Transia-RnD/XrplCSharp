using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xrpl.Client;
using Xrpl.Models.Methods;
using Xrpl.Sugar;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/client/getFeeXrp.ts

namespace XrplTests.Xrpl.ClientLib
{
    [TestClass]
    public class TestUGetFeeXrp
    {
        public static SetupUnitClient runner;

        [TestInitialize]
        public async Task MyTestInitializeAsync()
        {
            runner = await new SetupUnitClient().SetupClient();
        }

        [TestCleanup]
        public async Task MyTestCleanupAsync()
        {
            await runner.client.Disconnect();
        }

        [TestMethod]
        public async Task TestGetFeeXrpDefault()
        {
            string jsonString = "{\"id\":0,\"status\":\"success\",\"type\":\"response\",\"result\":{\"info\":{\"build_version\":\"0.24.0-rc1\",\"complete_ledgers\":\"32570-6595042\",\"hostid\":\"ARTS\",\"io_latency_ms\":1,\"last_close\":{\"converge_time_s\":2.007,\"proposers\":4},\"load_factor\":1,\"peers\":53,\"pubkey_node\":\"n94wWvFUmaKGYrKUGgpv1DyYgDeXRGdACkNQaSe7zJiy5Znio7UC\",\"server_state\":\"full\",\"validated_ledger\":{\"age\":5,\"base_fee_xrp\":0.00001,\"hash\":\"4482DEE5362332F54A4036ED57EE1767C9F33CF7CE5A6670355C16CECE381D46\",\"reserve_base_xrp\":20,\"reserve_inc_xrp\":5,\"seq\":6595042},\"validation_quorum\":3}}}";
            Dictionary<string, dynamic> jsonData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);
            runner.mockedRippled.AddResponse("server_info", jsonData);
            string fee = await GetFeeXrpSugar.GetFeeXrp(runner.client);
            Assert.AreEqual(fee, "0.000012");
        }
    }
}

