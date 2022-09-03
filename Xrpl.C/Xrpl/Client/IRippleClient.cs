using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Transactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Xrpl.Client.Exceptions;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Transactions;
using Xrpl.Sugar;
using Xrpl.XrplWallet;

using BookOffers = Xrpl.Client.Models.Transactions.BookOffers;
using ChannelAuthorize = Xrpl.Client.Models.Transactions.ChannelAuthorize;
using ChannelVerify = Xrpl.Client.Models.Transactions.ChannelVerify;
using Submit = Xrpl.Client.Models.Transactions.Submit;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/client/index.ts

namespace Xrpl.Client
{
    public interface IRippleClient
    {

        Task<AccountChannels> AccountChannels(AccountChannelsRequest request);
        Task<AccountCurrencies> AccountCurrencies(AccountCurrenciesRequest request);
        Task<AccountInfo> AccountInfo(AccountInfoRequest request);
        Task<AccountLines> AccountLines(AccountLinesRequest request);
        Task<AccountNFTs> AccountNFTs(AccountNFTsRequest request);
        Task<AccountObjects> AccountObjects(AccountObjectsRequest request);
        Task<AccountOffers> AccountOffers(AccountOffersRequest request);
        Task<AccountTransactions> AccountTransactions(AccountTransactionsRequest request);
        Task<BookOffers> BookOffers(BookOffersRequest request);
        //Task<DepositAuthorized> DepositAuthorized(DepositAuthorizedRequest request);
        Task<ChannelAuthorize> ChannelAuthorize(ChannelAuthorizeRequest request);
        Task<ChannelVerify> ChannelVerify(ChannelVerifyRequest request);
        Task<Fee> Fee(FeeRequest request);
        Task<GatewayBalances> GatewayBalances(GatewayBalancesRequest request);
        Task<LOLedger> Ledger(LedgerRequest request);
        Task<LOBaseLedger> LedgerClosed(LedgerClosedRequest request);
        Task<LOLedgerCurrentIndex> LedgerCurrent(LedgerCurrentRequest request);
        Task<LOLedgerData> LedgerData(LedgerDataRequest request);
        Task<LOLedgerEntry> LedgerEntry(LedgerEntryRequest request);
        Task<NFTBuyOffers> NFTBuyOffers(NFTBuyOffersRequest request);
        Task<NFTSellOffers> NFTSellOffers(NFTSellOffersRequest request);
        Task<NoRippleCheck> NoRippleCheck(NoRippleCheckRequest request);
        //Task<PathFind> PathFind(PathFindRequest request);
        Task<object> Ping(PingRequest request);
        Task<object> Random(RandomRequest request);
        //Task<RipplePathFind> RipplePathFind(RipplePathFindRequest request);
        Task<ServerInfo> ServerInfo(ServerInfoRequest request);
        // Task<ServerState> ServerState(ServerStateRequest request);
        Task<Submit> Submit(SubmitRequest request);
        //Task<SubmitMultisign> SubmitMultisign(SubmitMultisignRequest request);
        Task<object> Subscribe(SubscribeRequest request);
        Task<object> Unsubscribe(UnsubscribeRequest request);
        //Task<TransactionEntry> TransactionEntry(TransactionEntryRequest request);
        Task<TransactionResponseCommon> Tx(TxRequest request);
        Task<object> AnyRequest(RippleRequest request);

        // Sugars
        Task<Dictionary<string, dynamic>> Autofill(Dictionary<string, dynamic> tx);
        Task<Submit> Submit(Dictionary<string, dynamic> tx, Wallet wallet);
        Task<uint> GetLedgerIndex();

        void Connect();
        /// <summary> Disconnect from server </summary>
        void Disconnect();
    }

    public class RippleClient : IRippleClient
    {
        private readonly WebSocketClient client;
        private readonly ConcurrentDictionary<Guid, TaskInfo> tasks;
        private readonly JsonSerializerSettings serializerSettings;

        public RippleClient(string url)
        {
            tasks = new ConcurrentDictionary<Guid, TaskInfo>();
            serializerSettings = new JsonSerializerSettings();
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            client = WebSocketClient.Create(url);
            client.OnMessageReceived(MessageReceived);
            client.OnConnectionError(Error);
        }

        // SUGARS
        public Task<Dictionary<string, dynamic>> Autofill(Dictionary<string, dynamic> tx)
        {
            return AutofillSugar.Autofill(this, tx, null);
        }
        public Task<Submit> Submit(Dictionary<string, dynamic> tx, Wallet wallet)
        {
            return SubmitSugar.Submit(this, tx, true, false, wallet);
        }
        public Task<uint> GetLedgerIndex()
        {
            return GetLedgerSugar.GetLedgerIndex(this);
        }
        /// <inheritdoc />
        public void Connect()
        {
            client.OnMessageReceived(MessageReceived);
            client.Connect();
            do
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            } while (client.State != WebSocketState.Open);
        }
        /// <inheritdoc />
        public void Disconnect()
        {
            client.Disconnect();
        }

        // REQUESTS
        public Task<AccountChannels> AccountChannels(AccountChannelsRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<AccountChannels> task = new TaskCompletionSource<AccountChannels>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(AccountChannels);

            tasks.TryAdd(request.Id, taskInfo);

            Debug.WriteLine(command);
            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountCurrencies> AccountCurrencies(AccountCurrenciesRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<AccountCurrencies> task = new TaskCompletionSource<AccountCurrencies>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(AccountCurrencies);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountInfo> AccountInfo(AccountInfoRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<AccountInfo> task = new TaskCompletionSource<AccountInfo>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(AccountInfo);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountLines> AccountLines(AccountLinesRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<AccountLines> task = new TaskCompletionSource<AccountLines>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(AccountLines);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }


        public Task<AccountNFTs> AccountNFTs(AccountNFTsRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<AccountNFTs> task = new TaskCompletionSource<AccountNFTs>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(AccountNFTs);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountObjects> AccountObjects(AccountObjectsRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<AccountObjects> task = new TaskCompletionSource<AccountObjects>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(AccountObjects);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountOffers> AccountOffers(AccountOffersRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<AccountOffers> task = new TaskCompletionSource<AccountOffers>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(AccountOffers);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountTransactions> AccountTransactions(AccountTransactionsRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<AccountTransactions> task = new TaskCompletionSource<AccountTransactions>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(AccountTransactions);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<BookOffers> BookOffers(BookOffersRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<BookOffers> task = new TaskCompletionSource<BookOffers>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(BookOffers);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<ChannelAuthorize> ChannelAuthorize(ChannelAuthorizeRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<ChannelAuthorize> task = new TaskCompletionSource<ChannelAuthorize>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(ChannelAuthorize);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<ChannelVerify> ChannelVerify(ChannelVerifyRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<ChannelVerify> task = new TaskCompletionSource<ChannelVerify>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(ChannelVerify);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        //public Task<DepositAuthorized> DepositAuthorized(DepositAuthorizedRequest request)
        //{
        //    var command = JsonConvert.SerializeObject(request, serializerSettings);
        //    TaskCompletionSource<DepositAuthorized> task = new TaskCompletionSource<DepositAuthorized>();

        //    TaskInfo taskInfo = new TaskInfo();
        //    taskInfo.TaskId = request.Id;
        //    taskInfo.TaskCompletionResult = task;
        //    taskInfo.Type = typeof(DepositAuthorized);

        //    tasks.TryAdd(request.Id, taskInfo);

        //    client.SendMessage(command);
        //    return task.Task;
        //}



        public Task<LOLedger> Ledger(LedgerRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<LOLedger> task = new TaskCompletionSource<LOLedger>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = Guid.Parse("1A3B944E-3632-467B-A53A-206305310BAE");
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(LOLedger);

            Debug.WriteLine(command);
            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<LOBaseLedger> LedgerClosed(LedgerClosedRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<LOBaseLedger> task = new TaskCompletionSource<LOBaseLedger>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(LOBaseLedger);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;

        }

        public Task<LOLedgerCurrentIndex> LedgerCurrent(LedgerCurrentRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<LOLedgerCurrentIndex> task = new TaskCompletionSource<LOLedgerCurrentIndex>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(LOLedgerCurrentIndex);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }
        /// <inheritdoc />
        public Task<LOLedgerData> LedgerData(LedgerDataRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<LOLedgerData> task = new TaskCompletionSource<LOLedgerData>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(LOLedgerData);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<LOLedgerEntry> LedgerEntry(LedgerEntryRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<LOLedgerEntry> task = new TaskCompletionSource<LOLedgerEntry>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(LOLedgerEntry);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<Fee> Fee(FeeRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<Fee> task = new TaskCompletionSource<Fee>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(Fee);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<GatewayBalances> GatewayBalances(GatewayBalancesRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<GatewayBalances> task = new TaskCompletionSource<GatewayBalances>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(GatewayBalances);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<NFTBuyOffers> NFTBuyOffers(NFTBuyOffersRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<NFTBuyOffers> task = new TaskCompletionSource<NFTBuyOffers>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(NFTBuyOffers);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<NFTSellOffers> NFTSellOffers(NFTSellOffersRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<NFTSellOffers> task = new TaskCompletionSource<NFTSellOffers>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(NFTSellOffers);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<NoRippleCheck> NoRippleCheck(NoRippleCheckRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<NoRippleCheck> task = new TaskCompletionSource<NoRippleCheck>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(NoRippleCheck);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        //public Task<PathFind> PathFind(PathFindRequest request)
        //{
        //    var command = JsonConvert.SerializeObject(request, serializerSettings);
        //    TaskCompletionSource<PathFind> task = new TaskCompletionSource<PathFind>();

        //    TaskInfo taskInfo = new TaskInfo();
        //    taskInfo.TaskId = request.Id;
        //    taskInfo.TaskCompletionResult = task;
        //    taskInfo.Type = typeof(PathFind);

        //    tasks.TryAdd(request.Id, taskInfo);

        //    client.SendMessage(command);
        //    return task.Task;
        //}

        public Task<object> Ping(PingRequest request)
        {
            //RippleRequest request = new RippleRequest();
            //request.Command = "ping";

            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<object> task = new TaskCompletionSource<object>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(object);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<object> Random(RandomRequest request)
        {
            //RippleRequest request = new RippleRequest();
            //request.Command = "ping";

            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<object> task = new TaskCompletionSource<object>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(object);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        //public Task<RipplePathFind> RipplePathFind(RipplePathFindRequest request)
        //{
        //    //RippleRequest request = new RippleRequest();
        //    //request.Command = "ping";

        //    var command = JsonConvert.SerializeObject(request, serializerSettings);
        //    TaskCompletionSource<RipplePathFind> task = new TaskCompletionSource<RipplePathFind>();

        //    TaskInfo taskInfo = new TaskInfo();
        //    taskInfo.TaskId = request.Id;
        //    taskInfo.TaskCompletionResult = task;
        //    taskInfo.Type = typeof(RipplePathFind);

        //    tasks.TryAdd(request.Id, taskInfo);

        //    client.SendMessage(command);
        //    return task.Task;
        //}

        public Task<ServerInfo> ServerInfo(ServerInfoRequest request)
        {
            /// <inheritdoc />
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<ServerInfo> task = new TaskCompletionSource<ServerInfo>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(ServerInfo);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        //public Task<ServerState> ServerState(ServerStateRequest request)
        //{
        //    /// <inheritdoc />
        //    var command = JsonConvert.SerializeObject(request, serializerSettings);
        //    TaskCompletionSource<ServerState> task = new TaskCompletionSource<ServerState>();

        //    TaskInfo taskInfo = new TaskInfo();
        //    taskInfo.TaskId = request.Id;
        //    taskInfo.TaskCompletionResult = task;
        //    taskInfo.Type = typeof(ServerState);

        //    tasks.TryAdd(request.Id, taskInfo);

        //    client.SendMessage(command);
        //    return task.Task;
        //}

        public Task<Submit> Submit(SubmitRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<Submit> task = new TaskCompletionSource<Submit>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(Submit);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        //public Task<SubmitMultisign> SubmitMultisign(SubmitMultisignRequest request, Wallet wallet)
        //{
        //    var command = JsonConvert.SerializeObject(request, serializerSettings);
        //    TaskCompletionSource<SubmitMultisign> task = new TaskCompletionSource<SubmitMultisign>();

        //    TaskInfo taskInfo = new TaskInfo();
        //    taskInfo.TaskId = request.Id;
        //    taskInfo.TaskCompletionResult = task;
        //    taskInfo.Type = typeof(SubmitMultisign);

        //    tasks.TryAdd(request.Id, taskInfo);

        //    client.SendMessage(command);
        //    return task.Task;
        //}

        public Task<object> Subscribe(SubscribeRequest request)
        {

            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<object> task = new TaskCompletionSource<object>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.RemoveUponCompletion = false;
            taskInfo.Type = typeof(object);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<object> Unsubscribe(UnsubscribeRequest request)
        {

            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<object> task = new TaskCompletionSource<object>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.RemoveUponCompletion = false;
            taskInfo.Type = typeof(object);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        //public Task<TransactionEntry> TransactionEntry(TransactionEntryRequest request)
        //{
        //    var command = JsonConvert.SerializeObject(request, serializerSettings);
        //    TaskCompletionSource<TransactionEntry> task = new TaskCompletionSource<TransactionEntry>();

        //    TaskInfo taskInfo = new TaskInfo();
        //    taskInfo.TaskId = request.Id;
        //    taskInfo.TaskCompletionResult = task;
        //    taskInfo.Type = typeof(TransactionEntry);

        //    tasks.TryAdd(request.Id, taskInfo);

        //    client.SendMessage(command);
        //    return task.Task;
        //}

        public Task<TransactionResponseCommon> Tx(TxRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<TransactionResponseCommon> task = new TaskCompletionSource<TransactionResponseCommon>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(TransactionResponseCommon);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<object> AnyRequest(RippleRequest request)
        {

            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<object> task = new TaskCompletionSource<object>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(RippleRequest);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        private void Error(Exception ex, WebSocketClient client)
        {
            throw new Exception(ex.Message, ex);
        }

        private void MessageReceived(string s, WebSocketClient client)
        {
            RippleResponse response = JsonConvert.DeserializeObject<RippleResponse>(s);
            try
            {
                var taskInfoResult = tasks.TryGetValue(response.Id, out var taskInfo);
                if (taskInfoResult == false) throw new Exception("Task not found");

                if (response.Status == "success")
                {
                    Debug.WriteLine($"RESPONSE {response.Id} : {response.Result.ToString()}");
                    var deserialized = JsonConvert.DeserializeObject(response.Result.ToString(), taskInfo.Type, serializerSettings);
                    var setResult = taskInfo.TaskCompletionResult.GetType().GetMethod("SetResult");
                    setResult.Invoke(taskInfo.TaskCompletionResult, new[] { deserialized });

                    if (taskInfo.RemoveUponCompletion)
                    {
                        tasks.TryRemove(response.Id, out taskInfo);
                    }
                }
                else if (response.Status == "error")
                {
                    var setException = taskInfo.TaskCompletionResult.GetType().GetMethod("SetException", new Type[] { typeof(Exception) }, null);

                    RippleException exception = new RippleException(response.Error);
                    setException.Invoke(taskInfo.TaskCompletionResult, new[] { exception });

                    tasks.TryRemove(response.Id, out taskInfo);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var taskInfoResult = tasks.TryGetValue(response.Id, out var taskInfo);
                var setException = taskInfo.TaskCompletionResult.GetType().GetMethod("SetException", new Type[] { typeof(Exception) }, null);

                RippleException exception = new RippleException(response.Error ?? e.Message, e);
                setException.Invoke(taskInfo.TaskCompletionResult, new[] { exception });

                tasks.TryRemove(response.Id, out taskInfo);
            }
        }
    }
}
