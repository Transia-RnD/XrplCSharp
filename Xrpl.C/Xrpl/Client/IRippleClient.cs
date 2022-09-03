using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
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

        // New Functions matching XRPLF

        Task<Dictionary<string, dynamic>> Autofill(Dictionary<string, dynamic> tx);

        Task<Submit> Submit(Dictionary<string, dynamic> tx, Wallet wallet);

        Task<uint> GetLedgerIndex();

        void Connect();
        /// <summary> Disconnect from server </summary>
        void Disconnect();

        Task<object> AnyRequest(RippleRequest request);

        Task<object> Subscribe();
        /// <summary> The subscribe method requests periodic notifications from the server when certain events happen. </summary>
        /// <param name="request">An <see cref="Xrpl.Client.Models.Methods.SubscribeRequest"/> request.</param>
        /// <returns></returns>
        Task<object> Subscribe(SubscribeRequest request);

        /// <summary> The ping command returns an acknowledgement,
        /// so that clients can test the connection status and latency </summary>
        /// <returns></returns>
        Task Ping();

        Task<AccountCurrencies> AccountCurrencies(string account);

        Task<AccountCurrencies> AccountCurrencies(AccountCurrenciesRequest request);

        Task<AccountChannels> AccountChannels(string account);

        Task<AccountChannels> AccountChannels(AccountChannelsRequest request);

        Task<AccountInfo> AccountInfo(string account);

        /// <summary> The account_info command retrieves information about an account, its activity, and its XRP balance. </summary>
        /// <param name="request">An <see cref="Xrpl.Client.Models.Methods.AccountInfoRequest"/> request.</param>
        /// <returns>An <see cref="Xrpl.Client.Models.Methods.AccountInfo"/> response.</returns>
        Task<AccountInfo> AccountInfo(AccountInfoRequest request);



        #endregion

        #region Offers

        /// <summary> The account_offers method retrieves a list of offers made by a given account that are outstanding as of a particular ledger version </summary>
        /// <param name="account">The unique identifier of an account, typically the account's Address.</param>
        /// <returns>An <see cref="Xrpl.Client.Models.Methods.AccountOffers"/> response.</returns>
        Task<AccountOffers> AccountOffers(string account);

        /// <summary> The account_offers method retrieves a list of offers made by a given account that are outstanding as of a particular ledger version </summary>
        /// <param name="request">An <see cref="Xrpl.Client.Models.Methods.AccountOffersRequest"/> request.</param>
        /// <returns>An <see cref="Xrpl.Client.Models.Methods.AccountOffers"/> response.</returns>
        Task<AccountOffers> AccountOffers(AccountOffersRequest request);

        #endregion

        #region Currencies

        /// <summary> The account_currencies command retrieves a list of currencies that an account can send or receive, based on its trust lines. </summary>
        /// <param name="account">The unique identifier of an account, typically the account's Address.</param>
        /// <returns>An <see cref="Xrpl.Client.Models.Methods.AccountCurrencies"/> response.</returns>
        Task<AccountCurrencies> AccountCurrencies(string account);

        /// <summary> The account_currencies command retrieves a list of currencies that an account can send or receive, based on its trust lines. </summary>
        /// <param name="request">An <see cref="Xrpl.Client.Models.Methods.AccountCurrenciesRequest"/> request.</param>
        /// <returns>An <see cref="Xrpl.Client.Models.Methods.AccountCurrencies"/> response.</returns>
        Task<AccountCurrencies> AccountCurrencies(AccountCurrenciesRequest request);

        #endregion

        #region Trustlines

        /// <summary>
        /// The account_lines method returns information about an account's trust lines, including balances in all non-XRP currencies and assets.
        /// </summary>
        /// <param name="account">The account number to query.</param>
        /// <returns>An <see cref="Xrpl.Client.Models.Methods.AccountLines"/> response.</returns>
        Task<AccountLines> AccountLines(string account);

        /// <summary>
        /// The account_lines method returns information about an account's trust lines, including balances in all non-XRP currencies and assets.
        /// </summary>
        /// <param name="request">An <see cref="Xrpl.Client.Models.Methods.AccountLinesRequest"/> request.</param>
        /// <returns>An <see cref="Xrpl.Client.Models.Methods.AccountLines"/> response.</returns>
        Task<AccountLines> AccountLines(AccountLinesRequest request);

        #endregion

        #region Objects

        /// <summary>
        /// The AccountObjects command returns the raw ledger format for all objects owned by an account. For a higher-level view of an account's trust lines and balances, see <see cref="Model.Account.AccountLines"/> instead.
        /// </summary>
        /// <param name="account"></param>
        /// <returns>An <see cref="Xrpl.Client.Models.Methods.AccountObjects"/> response.</returns>
        Task<AccountObjects> AccountObjects(string account);

        /// <summary>
        /// The AccountObjects command returns the raw ledger format for all objects owned by an account. For a higher-level view of an account's trust lines and balances, see <see cref="Model.Account.AccountLines"/> instead.
        /// </summary>
        /// <param name="request">An <see cref="Xrpl.Client.Models.Methods.AccountObjectsRequest"/> request.</param>
        /// <returns>An <see cref="Xrpl.Client.Models.Methods.AccountObjects"/> response.</returns>
        Task<AccountObjects> AccountObjects(AccountObjectsRequest request);

        Task<AccountNFTs> AccountNFTs(string account);

        Task<AccountNFTs> AccountNFTs(AccountNFTsRequest request);

        Task<AccountTransactions> AccountTransactions(string account);

        Task<AccountTransactions> AccountTransactions(AccountTransactionsRequest request);

        Task<NoRippleCheck> NoRippleCheck(string account);

        /// <summary>
        /// The noripple_check command provides a quick way to check the status of the Default Ripple field
        /// for an account and the No Ripple flag of its trust lines, compared with the recommended settings
        /// </summary>
        /// <returns>An <see cref="Xrpl.Client.Models.Methods.NoRippleCheckRequest"/> response.</returns>
        /// <returns>An <see cref="Xrpl.Client.Models.Methods.NoRippleCheck"/> response.</returns>
        Task<NoRippleCheck> NoRippleCheck(NoRippleCheckRequest request);


        #endregion

        #region Balance

        /// <summary> The gateway_balances command calculates the total balances issued by a given account,
        /// optionally excluding amounts held by operational addresses. </summary>
        /// <param name="account">The unique identifier of an account, typically the account's Address.</param>
        /// <returns>An <see cref="Xrpl.Client.Models.Methods.GatewayBalances"/> response.</returns>
        Task<GatewayBalances> GatewayBalances(string account);

        /// <summary> The gateway_balances command calculates the total balances issued by a given account,
        /// optionally excluding amounts held by operational addresses. </summary>
        /// <param name="request">An <see cref="Xrpl.Client.Models.Methods.GatewayBalancesRequest"/> request.</param>
        /// <returns>An <see cref="Xrpl.Client.Models.Methods.GatewayBalances"/> response.</returns>
        Task<GatewayBalances> GatewayBalances(GatewayBalancesRequest request);

        Task<TransactionResponseCommon> Tx(TxRequest transaction);

        Task<ServerInfo> ServerInfo(ServerInfoRequest request);

        Task<Fee> Fee(FeeRequest request);

        Task<ChannelAuthorize> ChannelAuthorize(ChannelAuthorizeRequest request);

        /// <summary>The channel_verify method checks the validity of a signature that can be used to redeem a specific amount of XRP from a payment channel</summary>
        /// <param name="request">An <see cref="Xrpl.Client.Models.Methods.ChannelVerifyRequest"/> request.</param>
        /// <returns>An <see cref="Xrpl.Client.Models.Transactions.ChannelAuthorize"/> response.</returns>
        Task<ChannelVerify> ChannelVerify(ChannelVerifyRequest request);

        #endregion

        #region Ledger

        Task<Submit> SubmitTransaction(SubmitRequest request, Wallet wallet);

        /// <summary>
        /// The ledger_request command tells server to fetch a specific ledger version from its connected peers.
        /// This only works if one of the server's immediately-connected peers has that ledger.
        /// You may need to run the command several times to completely fetch a ledger
        /// </summary>
        /// <param name="request">An <see cref="Xrpl.Client.Models.Methods.LedgerRequest"/> request.</param>
        /// <returns>An <see cref="Xrpl.Client.Models.Ledger.LOLedger"/> response.</returns>
        Task<LOLedger> Ledger(LedgerRequest request);

        Task<LOBaseLedger> LedgerClosed(LedgerClosedRequest request);

        Task<LOLedgerCurrentIndex> LedgerCurrent(LedgerCurrentRequest request);

        Task<LOLedgerData> LedgerData(LedgerDataRequest request);

        Task<LOLedgerEntry> LedgerEntry(LedgerEntryRequest request);
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

        // ---------------------------------------------------------------
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

        public Task<object> Subscribe()
        {
            SubscribeRequest request = new SubscribeRequest();
            return Subscribe(request);
        }
        /// <inheritdoc />
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
        /// <inheritdoc />
        public Task Ping()
        {
            RippleRequest request = new RippleRequest();
            request.Command = "ping";

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

        public Task<AccountChannels> AccountChannels(string account)
        {
            AccountChannelsRequest request = new AccountChannelsRequest(account);
            return AccountChannels(request);
        }

            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<ServerInfo> task = new TaskCompletionSource<ServerInfo>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(ServerInfo);

            tasks.TryAdd(request.Id, taskInfo);

            Debug.WriteLine(command);
            client.SendMessage(command);
            return task.Task;
        }

        public Task<AccountCurrencies> AccountCurrencies(string account)
        {
            AccountCurrenciesRequest request = new AccountCurrenciesRequest(account);
            return AccountCurrencies(request);
        }

        public Task<AccountCurrencies> AccountCurrencies(AccountCurrenciesRequest request)
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


        #endregion

        #region Account

        #region Info

        /// <inheritdoc />
        public Task<AccountInfo> AccountInfo(string account)
        {
            AccountInfoRequest request = new AccountInfoRequest(account);
            return AccountInfo(request);
        }
        /// <inheritdoc />
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

        #endregion

        #region Offers

        /// <inheritdoc />
        public Task<AccountOffers> AccountOffers(string account)
        {
            AccountOffersRequest request = new AccountOffersRequest(account);
            return AccountOffers(request);
        }
        /// <inheritdoc />
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

        #endregion

        #region Currencies

        /// <inheritdoc />
        public Task<AccountCurrencies> AccountCurrencies(string account)
        {
            AccountCurrenciesRequest request = new AccountCurrenciesRequest(account);
            return AccountCurrencies(request);
        }
        /// <inheritdoc />
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

        #endregion

        #region Trustlines

        /// <inheritdoc />
        public Task<AccountLines> AccountLines(string account)
        {
            AccountLinesRequest request = new AccountLinesRequest(account);
            return AccountLines(request);
        }
        /// <inheritdoc />
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

        #endregion

        #region Objects

        /// <inheritdoc />
        public Task<AccountObjects> AccountObjects(string account)
        {
            AccountObjectsRequest request = new AccountObjectsRequest(account);
            return AccountObjects(request);
        }
        /// <inheritdoc />
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

        #endregion

        #region NoRipple

        /// <inheritdoc />
        public Task<NoRippleCheck> NoRippleCheck(string account)
        {
            NoRippleCheckRequest request = new NoRippleCheckRequest(account);
            return NoRippleCheck(request);
        }
        /// <inheritdoc />
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

        #endregion

        #region Balance

        /// <inheritdoc />
        public Task<GatewayBalances> GatewayBalances(string account)
        {
            GatewayBalancesRequest request = new GatewayBalancesRequest(account);
            return GatewayBalances(request);
        }
        /// <inheritdoc />
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

        #endregion

        #endregion

        #region NFT

        /// <inheritdoc />
        public Task<NFTBuyOffers> NFTBuyOffers(string nft_id)
        {
            NFTBuyOffersRequest request = new NFTBuyOffersRequest(nft_id);
            return NFTBuyOffers(request);
        }

        public Task<NoRippleCheck> NoRippleCheck(NoRippleCheckRequest request)
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
        /// <inheritdoc />
        public Task<AccountNFTs> AccountNFTs(string account)
        {
            AccountNFTsRequest request = new AccountNFTsRequest(account);
            return AccountNFTs(request);
        }
        /// <inheritdoc />
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

        public Task<TransactionResponseCommon> Tx(TxRequest request)
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
        /// <inheritdoc />
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

        #region Channels

        /// <inheritdoc />
        public Task<AccountChannels> AccountChannels(string account)
        {
            AccountChannelsRequest request = new AccountChannelsRequest(account);
            return AccountChannels(request);
        }

        public Task<Submit> SubmitTransactionBlob(SubmitBlobRequest request)
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

        public Task<Submit> SubmitTransaction(SubmitRequest request, Wallet wallet)
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
        /// <inheritdoc />
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

        #endregion

        #region Ledger

        /// <inheritdoc />
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
