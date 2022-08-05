using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xrpl.Client.Exceptions;
using Xrpl.Client.Models.Ledger;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Transactions;

using BookOffers = Xrpl.Client.Models.Transactions.BookOffers;
using ChannelAuthorize = Xrpl.Client.Models.Transactions.ChannelAuthorize;
using ChannelVerify = Xrpl.Client.Models.Transactions.ChannelVerify;
using Submit = Xrpl.Client.Models.Transactions.Submit;

namespace Xrpl.Client
{
    public interface IRippleClient
    {
        void Connect();

        void Disconnect();

        Task<object> Subscribe();

        Task<object> Subscribe(SubscribeRequest request);

        Task Ping();

        Task<AccountCurrencies> AccountCurrencies(string account);

        Task<AccountCurrencies> AccountCurrencies(AccountCurrenciesRequest request);

        Task<AccountChannels> AccountChannels(string account);

        Task<AccountChannels> AccountChannels(AccountChannelsRequest request);

        Task<AccountInfo> AccountInfo(string account);

        Task<AccountInfo> AccountInfo(AccountInfoRequest request);

        /// <summary>
        /// The account_lines method returns information about an account's trust lines, including balances in all non-XRP currencies and assets.
        /// </summary>
        /// <param name="account">The account number to query.</param>
        /// <returns>An <see cref="Models.Methods.AccountOffers"/> response.</returns>
        Task<AccountLines> AccountLines(string account);

        /// <summary>
        /// The account_lines method returns information about an account's trust lines, including balances in all non-XRP currencies and assets.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>An <see cref="Models.Methods.AccountOffers"/> response.</returns>
        Task<AccountLines> AccountLines(AccountLinesRequest request);

        Task<AccountOffers> AccountOffers(string account);

        Task<AccountOffers> AccountOffers(AccountOffersRequest request);

        Task<NFTBuyOffers> NFTBuyOffers(string nft_id);

        Task<NFTBuyOffers> NFTBuyOffers(NFTBuyOffersRequest request);

        Task<NFTSellOffers> NFTSellOffers(string nft_id);

        Task<NFTSellOffers> NFTSellOffers(NFTSellOffersRequest request);

        /// <summary>
        /// The AccountObjects command returns the raw ledger format for all objects owned by an account. For a higher-level view of an account's trust lines and balances, see <see cref="Model.Account.AccountLines"/> instead.
        /// </summary>
        /// <param name="account"></param>
        /// <returns>An <see cref="Models.Methods.AccountObjects"/> response.</returns>
        Task<AccountObjects> AccountObjects(string account);

        /// <summary>
        /// The AccountObjects command returns the raw ledger format for all objects owned by an account. For a higher-level view of an account's trust lines and balances, see <see cref="Model.Account.AccountLines"/> instead.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>An <see cref="Models.Methods.AccountObjects"/> response.</returns>
        Task<AccountObjects> AccountObjects(AccountObjectsRequest request);

        Task<AccountNFTs> AccountNFTs(string account);

        Task<AccountNFTs> AccountNFTs(AccountNFTsRequest request);

        Task<AccountTransactions> AccountTransactions(string account);

        Task<AccountTransactions> AccountTransactions(AccountTransactionsRequest request);

        Task<NoRippleCheck> NoRippleCheck(string account);

        Task<NoRippleCheck> NoRippleCheck(NoRippleCheckRequest request);

        Task<GatewayBalances> GatewayBalances(string account);

        Task<GatewayBalances> GatewayBalances(GatewayBalancesRequest request);

        Task<TransactionResponseCommon> TTransaction(string transaction);

        Task<ITransactionResponseCommon> Transaction(string transaction);

        Task<IBaseTransactionResponse> TransactionAsBinary(string transaction);

        Task<ServerInfo> ServerInfo();

        Task<Fee> Fees();

        Task<ChannelAuthorize> ChannelAuthorize(ChannelAuthorizeRequest request);

        Task<ChannelVerify> ChannelVerify(ChannelVerifyRequest request);

        Task<Submit> SubmitTransactionBlob(SubmitBlobRequest request);

        Task<Submit> SubmitTransaction(SubmitRequest request);

        Task<BookOffers> BookOffers(BookOffersRequest request);

        Task<LOLedger> Ledger(LedgerRequest request);

        Task<LOBaseLedger> ClosedLedger();

        Task<LOLedgerCurrentIndex> CurrentLedger();

        Task<LOLedgerData> LedgerData(LedgerDataRequest request);
    }

    public class RippleClient : IRippleClient
    {
        private readonly WebSocketClient client;
        private readonly ConcurrentDictionary<Guid, TaskInfo> tasks;
        private readonly JsonSerializerSettings serializerSettings;

        public RippleClient(string url)
        {
            tasks = new ConcurrentDictionary<Guid, TaskInfo>();
            serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };

            client = WebSocketClient.Create(url);
            client.OnMessageReceived(MessageReceived);
            client.OnConnectionError(Error);
        }

        public void Connect()
        {
            client.OnMessageReceived(MessageReceived);
            client.Connect();
            do
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            } while (client.State != WebSocketState.Open);
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        public Task<object> Subscribe()
        {
            var request = new SubscribeRequest();
            return Subscribe(request);
        }

        public Task<object> Subscribe(SubscribeRequest request)
        {

            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<object>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                RemoveUponCompletion = false,
                Type = typeof(object)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task Ping()
        {
            var request = new RippleRequest
            {
                Command = "ping"
            };

            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<object>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(object)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountChannels> AccountChannels(string account)
        {
            var request = new AccountChannelsRequest(account);
            return AccountChannels(request);
        }

        public Task<AccountChannels> AccountChannels(AccountChannelsRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<AccountChannels>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(AccountChannels)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountCurrencies> AccountCurrencies(string account)
        {
            var request = new AccountCurrenciesRequest(account);
            return AccountCurrencies(request);
        }

        public Task<AccountCurrencies> AccountCurrencies(AccountCurrenciesRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<AccountCurrencies>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(AccountCurrencies)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountInfo> AccountInfo(string account)
        {
            var request = new AccountInfoRequest(account);
            return AccountInfo(request);
        }

        public Task<AccountInfo> AccountInfo(AccountInfoRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<AccountInfo>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(AccountInfo)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountLines> AccountLines(string account)
        {
            var request = new AccountLinesRequest(account);
            return AccountLines(request);
        }

        public Task<AccountLines> AccountLines(AccountLinesRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<AccountLines>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(AccountLines)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountOffers> AccountOffers(string account)
        {
            var request = new AccountOffersRequest(account);
            return AccountOffers(request);
        }

        public Task<AccountOffers> AccountOffers(AccountOffersRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<AccountOffers>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(AccountOffers)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<NFTBuyOffers> NFTBuyOffers(string nft_id)
        {
            var request = new NFTBuyOffersRequest(nft_id);
            return NFTBuyOffers(request);
        }

        public Task<NFTBuyOffers> NFTBuyOffers(NFTBuyOffersRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<NFTBuyOffers>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(NFTBuyOffers)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<NFTSellOffers> NFTSellOffers(string nft_id)
        {
            var request = new NFTSellOffersRequest(nft_id);
            return NFTSellOffers(request);
        }

        public Task<NFTSellOffers> NFTSellOffers(NFTSellOffersRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<NFTSellOffers>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(NFTSellOffers)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountObjects> AccountObjects(string account)
        {
            var request = new AccountObjectsRequest(account);
            return AccountObjects(request);
        }

        public Task<AccountObjects> AccountObjects(AccountObjectsRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<AccountObjects>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(AccountObjects)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountNFTs> AccountNFTs(string account)
        {
            var request = new AccountNFTsRequest(account);
            return AccountNFTs(request);
        }

        public Task<AccountNFTs> AccountNFTs(AccountNFTsRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<AccountNFTs>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(AccountNFTs)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountTransactions> AccountTransactions(string account)
        {
            var request = new AccountTransactionsRequest(account);
            return AccountTransactions(request);
        }

        public Task<AccountTransactions> AccountTransactions(AccountTransactionsRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<AccountTransactions>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(AccountTransactions)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<NoRippleCheck> NoRippleCheck(string account)
        {
            var request = new NoRippleCheckRequest(account);
            return NoRippleCheck(request);
        }

        public Task<NoRippleCheck> NoRippleCheck(NoRippleCheckRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<NoRippleCheck>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(NoRippleCheck)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<GatewayBalances> GatewayBalances(string account)
        {
            var request = new GatewayBalancesRequest(account);
            return GatewayBalances(request);
        }

        public Task<GatewayBalances> GatewayBalances(GatewayBalancesRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<GatewayBalances>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(GatewayBalances)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<TransactionResponseCommon> TTransaction(string transaction)
        {
            var request = new TransactionRequest(transaction);
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<TransactionResponseCommon>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(TransactionResponseCommon)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<ITransactionResponseCommon> Transaction(string transaction)
        {
            var request = new TransactionRequest(transaction);
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<ITransactionResponseCommon>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(TransactionResponseCommon)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<IBaseTransactionResponse> TransactionAsBinary(string transaction)
        {
            var request = new TransactionRequest(transaction)
            {
                Binary = true
            };
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<IBaseTransactionResponse>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(BinaryTransactionResponse)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;

        }

        public Task<ServerInfo> ServerInfo()
        {
            var request = new RippleRequest
            {
                Command = "server_info"
            };

            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<ServerInfo>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(ServerInfo)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<Fee> Fees()
        {
            var request = new RippleRequest
            {
                Command = "fee"
            };

            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<Fee>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(Fee)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<ChannelAuthorize> ChannelAuthorize(ChannelAuthorizeRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<ChannelAuthorize>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(ChannelAuthorize)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<ChannelVerify> ChannelVerify(ChannelVerifyRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<ChannelVerify>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(ChannelVerify)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<Submit> SubmitTransactionBlob(SubmitBlobRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<Submit>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(Submit)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<Submit> SubmitTransaction(SubmitRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<Submit>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(Submit)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<BookOffers> BookOffers(BookOffersRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<BookOffers>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(BookOffers)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<LOLedger> Ledger(LedgerRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<LOLedger>();

            var taskInfo = new TaskInfo
            {
                TaskId = Guid.Parse("1A3B944E-3632-467B-A53A-206305310BAE"),
                TaskCompletionResult = task,
                Type = typeof(LOLedger)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<LOBaseLedger> ClosedLedger()
        {
            var request = new ClosedLedgerRequest();
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<LOBaseLedger>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(LOBaseLedger)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;

        }

        public Task<LOLedgerCurrentIndex> CurrentLedger()
        {
            var request = new CurrentLedgerRequest();
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<LOLedgerCurrentIndex>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(LOLedgerCurrentIndex)
            };

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        public Task<LOLedgerData> LedgerData(LedgerDataRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            var task = new TaskCompletionSource<LOLedgerData>();

            var taskInfo = new TaskInfo
            {
                TaskId = request.Id,
                TaskCompletionResult = task,
                Type = typeof(LOLedgerData)
            };

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
            var response = JsonConvert.DeserializeObject<RippleResponse>(s);
            try
            {
                var taskInfoResult = tasks.TryGetValue(response.Id, out var taskInfo);
                if (taskInfoResult == false) throw new Exception("Task not found");

                switch (response.Status)
                {
                    case "success":
                    {
                        var deserialized = JsonConvert.DeserializeObject(response.Result.ToString(), taskInfo.Type, serializerSettings);
                        var setResult = taskInfo.TaskCompletionResult.GetType().GetMethod("SetResult");
                        setResult.Invoke(taskInfo.TaskCompletionResult, new[] { deserialized });

                        if (taskInfo.RemoveUponCompletion)
                        {
                            tasks.TryRemove(response.Id, out taskInfo);
                        }

                        break;
                    }
                    case "error":
                    {
                        var setException = taskInfo.TaskCompletionResult.GetType().GetMethod("SetException", new Type[] { typeof(Exception) }, null);

                        var exception = new RippleException(response.Error);
                        setException.Invoke(taskInfo.TaskCompletionResult, new[] { exception });

                        tasks.TryRemove(response.Id, out taskInfo);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var taskInfoResult = tasks.TryGetValue(response.Id, out var taskInfo);
                var setException = taskInfo.TaskCompletionResult.GetType().GetMethod("SetException", new Type[] { typeof(Exception) }, null);

                var exception = new RippleException(response.Error ?? e.Message, e);
                setException.Invoke(taskInfo.TaskCompletionResult, new[] { exception });

                tasks.TryRemove(response.Id, out taskInfo);
            }
        }
    }
}
