using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading.Tasks;

using Xrpl.ClientLib.Exceptions;
using Xrpl.Models.Ledger;
using Xrpl.Models.Methods;
using Xrpl.Models.Subscriptions;
using Xrpl.Models.Transactions;
using Xrpl.Sugar;
using Xrpl.WalletLib;

using BookOffers = Xrpl.Models.Transactions.BookOffers;
using Submit = Xrpl.Models.Transactions.Submit;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/client/index.ts
// https://xrpl.org/public-api-methods.html
namespace Xrpl.ClientLib
{
    public delegate void OnRippleResponse(string response);
    public delegate void OnLedgerStreamResponse(LedgerStreamResponseResult response);
    public delegate void OnValidationsStreamResponse(ValidationsStreamResponseResult response);
    public delegate void OnTransactionStreamResponse(TransactionStreamResponseResult response);
    public delegate void OnPeerStatusStreamResponse(PeerStatusStreamResponseResult response);
    public delegate void OnConsensusStreamResponse(ConsensusStreamResponseResult response);
    public delegate void OnPathFindStream(PathFindStreamResult response);
    public delegate void OnErrorResponse(ErrorResponse response);

    public interface IClient : IDisposable
    {
        event OnLedgerStreamResponse OnLedgerClosed;
        event OnValidationsStreamResponse OnValidation;
        event OnTransactionStreamResponse OnTransaction;
        event OnPeerStatusStreamResponse OnPeerStatusChange;
        event OnConsensusStreamResponse OnConsensusPhase;
        event OnPathFindStream OnPathFind;
        event OnErrorResponse OnError;
        event OnRippleResponse OnResponse;

        #region Server
        /// <summary> connect to the server </summary>
        void Connect();
        /// <summary> Disconnect from server </summary>
        void Disconnect();
        /// <summary> The subscribe method requests periodic notifications from the server when certain events happen. </summary>
        /// <param name="request">An <see cref="SubscribeRequest"/> request.</param>
        /// <returns></returns>
        Task<object> Subscribe(SubscribeRequest request);
        /// <summary> The unsubscribe command tells the server to stop sending messages for a particular subscription or set of subscriptions.</summary>
        /// <param name="request">An <see cref="UnsubscribeRequest"/> request.</param>
        /// <returns></returns>
        Task<object> Unsubscribe(UnsubscribeRequest request);
        /// <summary>
        /// The ping command returns an acknowledgement,
        /// so that clients can test the connection status and latency
        /// </summary>
        /// <param name="request">An <see cref="PingRequest"/> request.</param>
        /// <returns></returns>
        Task<object> Ping(PingRequest request);
        /// <summary> The server_info command asks the server for a human-readable version of various information about the rippled server being queried. </summary>
        /// <param name="request">An <see cref="ServerInfoRequest"/> request.</param>
        /// <returns>A <see cref="ServerInfo"/> response.</returns>
        Task<ServerInfo> ServerInfo(ServerInfoRequest request);
        /// <summary> The fee command reports the current state of the open-ledger requirements for the transaction cost. </summary>
        /// <param name="request">An <see cref="FeeRequest"/> request.</param>
        /// <returns>An <see cref="Models.Methods.Fee"/> response.</returns>
        Task<Fee> Fee(FeeRequest request);

        #endregion

        #region Account
        //https://xrpl.org/account-methods.html
        /// <summary> The account_info command retrieves information about an account, its activity, and its XRP balance. </summary>
        /// <param name="request">An <see cref="AccountInfoRequest"/> request.</param>
        /// <returns>An <see cref="Models.Methods.AccountInfo"/> response.</returns>
        Task<AccountInfo> AccountInfo(AccountInfoRequest request);


        /// <summary> The account_offers method retrieves a list of offers made by a given account that are outstanding as of a particular ledger version </summary>
        /// <param name="request">An <see cref="AccountOffersRequest"/> request.</param>
        /// <returns>An <see cref="Models.Methods.AccountOffers"/> response.</returns>
        Task<AccountOffers> AccountOffers(AccountOffersRequest request);

        /// <summary> The account_currencies command retrieves a list of currencies that an account can send or receive, based on its trust lines. </summary>
        /// <param name="request">An <see cref="AccountCurrenciesRequest"/> request.</param>
        /// <returns>An <see cref="Models.Methods.AccountCurrencies"/> response.</returns>
        Task<AccountCurrencies> AccountCurrencies(AccountCurrenciesRequest request);


        /// <summary>
        /// The account_lines method returns information about an account's trust lines, including balances in all non-XRP currencies and assets.
        /// </summary>
        /// <param name="request">An <see cref="AccountLinesRequest"/> request.</param>
        /// <returns>An <see cref="Models.Methods.AccountLines"/> response.</returns>
        Task<AccountLines> AccountLines(AccountLinesRequest request);


        /// <summary>
        /// The AccountObjects command returns the raw ledger format for all objects owned by an account. For a higher-level view of an account's trust lines and balances, see <see cref="Models.Methods.AccountLines"/> instead.
        /// </summary>
        /// <param name="request">An <see cref="AccountObjectsRequest"/> request.</param>
        /// <returns>An <see cref="Models.Methods.AccountObjects"/> response.</returns>
        Task<AccountObjects> AccountObjects(AccountObjectsRequest request);


        /// <summary>
        /// The noripple_check command provides a quick way to check the status of the Default Ripple field
        /// for an account and the No Ripple flag of its trust lines, compared with the recommended settings
        /// </summary>
        /// <returns>An <see cref="NoRippleCheckRequest"/> response.</returns>
        /// <returns>An <see cref="Models.Methods.NoRippleCheck"/> response.</returns>
        Task<NoRippleCheck> NoRippleCheck(NoRippleCheckRequest request);


        /// <summary> The gateway_balances command calculates the total balances issued by a given account,
        /// optionally excluding amounts held by operational addresses. </summary>
        /// <param name="request">An <see cref="GatewayBalancesRequest"/> request.</param>
        /// <returns>An <see cref="Models.Methods.GatewayBalances"/> response.</returns>
        Task<GatewayBalances> GatewayBalances(GatewayBalancesRequest request);


        /// <summary> The account_tx method retrieves a list of transactions that involved the specified account </summary>
        /// <param name="request">An <see cref="AccountTransactionsRequest"/> request.</param>
        /// <returns>An <see cref="Models.Methods.AccountTransactions"/> response.</returns>
        Task<AccountTransactions> AccountTransactions(AccountTransactionsRequest request);
        /// <summary> The account_channels method returns information about an account's Payment Channels.
        /// This includes only channels where the specified account is the channel's source, not the destination. </summary>
        /// <param name="request">An <see cref="AccountChannelsRequest"/> request.</param>
        /// <returns>An <see cref="Models.Methods.AccountChannels"/> response.</returns>
        Task<AccountChannels> AccountChannels(AccountChannelsRequest request);

        #endregion

        #region NFT


        /// <summary> The nft_buy_offers method returns a list of buy offers for a given NFToken object. </summary>
        /// <param name="request">An <see cref="NFTBuyOffersRequest"/> request.</param>
        /// <returns>An <see cref="Models.Methods.NFTBuyOffers"/> response.</returns>
        Task<NFTBuyOffers> NFTBuyOffers(NFTBuyOffersRequest request);

        /// <summary> The nft_sell_offers method returns a list of sell offers for a given NFToken object</summary>
        /// <param name="request">An <see cref="NFTSellOffersRequest"/> request.</param>
        /// <returns>An <see cref="Models.Methods.NFTSellOffers"/> response.</returns>
        Task<NFTSellOffers> NFTSellOffers(NFTSellOffersRequest request);


        /// <summary> The account_nfts method returns a list of NFToken objects for the specified account.</summary>
        /// <param name="request">An <see cref="AccountNFTsRequest"/> request.</param>
        /// <returns>An <see cref="Models.Methods.AccountNFTs"/> response.</returns>
        Task<AccountNFTs> AccountNFTs(AccountNFTsRequest request);


        #endregion

        #region Transactions
        //https://xrpl.org/transaction-methods.html
        /// <summary>
        /// The submit method applies a transaction and sends it to the network to be confirmed and included in future ledgers.
        /// </summary>
        /// <param name="request">An <see cref="SubmitRequest"/> request.</param>
        /// <returns>An <see cref="Xrpl.Models.Transactions.Submit"/> response.</returns>
        Task<Submit> Submit(SubmitRequest request);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tx">
        /// Transaction in JSON format with an array of Signers.<br/>
        /// To be successful, the weights of the signatures must be equal or higher than the quorum of the SignerList.
        /// </param>
        /// <param name="wallet"></param>//todo add description
        /// <returns>An <see cref="Xrpl.Models.Transactions.Submit"/> response.</returns>
        Task<Submit> Submit(Dictionary<string, dynamic> tx, Wallet wallet);
        /// <summary>
        /// The tx method retrieves information on a single transaction, by its identifying hash
        /// </summary>
        /// <param name="request">An <see cref="TxRequest"/> request.</param>
        /// <returns>An <see cref="TransactionResponseCommon"/> response.</returns>
        Task<TransactionResponseCommon> Tx(TxRequest request);

        #endregion

        #region Channels


        #endregion

        #region Ledger
        //https://xrpl.org/ledger-methods.html

        /// <summary>
        /// The ledger_request command tells server to fetch a specific ledger version from its connected peers.
        /// This only works if one of the server's immediately-connected peers has that ledger.
        /// You may need to run the command several times to completely fetch a ledger
        /// </summary>
        /// <param name="request">An <see cref="LedgerRequest"/> request.</param>
        /// <returns>An <see cref="LOLedger"/> response.</returns>
        Task<LOLedger> Ledger(LedgerRequest request);

        /// <summary>
        /// The ledger_data method retrieves contents of the specified ledger.
        /// You can iterate through several calls to retrieve the entire contents of a single ledger version.
        /// </summary>
        /// <param name="request">An <see cref="LedgerDataRequest"/> request.</param>
        /// <returns>An <see cref="LOLedgerData"/> response.</returns>
        Task<LOLedgerData> LedgerData(LedgerDataRequest request);
        /// <summary> The ledger_closed method returns the unique identifiers of the most recently closed ledger. </summary>
        /// <param name="request">An <see cref="LedgerClosedRequest"/> response.</param>
        /// <returns>An <see cref="LOBaseLedger"/> response.</returns>
        Task<LOBaseLedger> LedgerClosed(LedgerClosedRequest request);
        /// <summary>
        /// The ledger_current method returns the unique identifiers of the current in-progress ledger.<br/>
        /// This command is mostly useful for testing, because the ledger returned is still in flux.
        /// </summary>
        /// <param name="request">An <see cref="LedgerCurrentRequest"/> response.</param>
        /// <returns>An <see cref="LOLedgerCurrentIndex"/> response.</returns>
        Task<LOLedgerCurrentIndex> LedgerCurrent(LedgerCurrentRequest request);
        /// <summary>
        /// The ledger_entry method returns a single ledger object from the XRP Ledger in its raw format.<br/>
        /// See ledger format for information on the different types of objects you can retrieve.
        /// </summary>
        /// <param name="request">An <see cref="LedgerEntryRequest"/> response.</param>
        /// <returns>An <see cref="LOLedgerEntry"/> response.</returns>
        Task<LOLedgerEntry> LedgerEntry(LedgerEntryRequest request);


        #endregion
        /// <summary>
        /// The book_offers method retrieves a list of offers, also known as the order book , between two currencies
        /// </summary>
        /// <param name="request">An <see cref="BookOffersRequest"/> request.</param>
        /// <returns>An <see cref="Models.Transactions.BookOffers"/> response.</returns>
        Task<BookOffers> BookOffers(BookOffersRequest request);
        /// <summary>
        /// The random command provides a random number to be used as a source of entropy for random number generation by clients.<br/>
        /// https://xrpl.org/random.html#random
        /// </summary>
        /// <param name="request">An <see cref="RandomRequest"/> request.</param>
        /// <returns></returns>
        Task<object> Random(RandomRequest request);

        //Task<DepositAuthorized> DepositAuthorized(DepositAuthorizedRequest request);
        //Task<PathFind> PathFind(PathFindRequest request);
        //Task<RipplePathFind> RipplePathFind(RipplePathFindRequest request);
        // Task<ServerState> ServerState(ServerStateRequest request);
        //Task<SubmitMultisign> SubmitMultisign(SubmitMultisignRequest request);
        //Task<TransactionEntry> TransactionEntry(TransactionEntryRequest request);
        Task<object> AnyRequest(RippleRequest request);

        Task<Dictionary<string, dynamic>> Request(Dictionary<string, dynamic> request);

        // Sugars
        Task<Dictionary<string, dynamic>> Autofill(Dictionary<string, dynamic> tx);
        Task<uint> GetLedgerIndex();
        Task<string> GetXrpBalance(string address);

    }

    public class Client : IClient
    {
        public event OnLedgerStreamResponse OnLedgerClosed;
        public event OnValidationsStreamResponse OnValidation;
        public event OnTransactionStreamResponse OnTransaction;
        public event OnPeerStatusStreamResponse OnPeerStatusChange;
        public event OnConsensusStreamResponse OnConsensusPhase;
        public event OnPathFindStream OnPathFind;
        public event OnErrorResponse OnError;
        public event OnRippleResponse OnResponse;

        public readonly string url;
        private readonly WebSocketClient client;
        private readonly ConcurrentDictionary<Guid, TaskInfo> tasks;
        private readonly JsonSerializerSettings serializerSettings;

        public Client(string server)
        {
            url = server;
            tasks = new ConcurrentDictionary<Guid, TaskInfo>();
            serializerSettings = new JsonSerializerSettings();
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            client = WebSocketClient.Create(server);
            OnResponse += OnMessageReceived;
            client.OnMessageReceived(MessageReceived);
            client.OnConnectionError(Error);
        }

        // SUGARS
        public Task<Dictionary<string, dynamic>> Autofill(Dictionary<string, dynamic> tx)
        {
            return AutofillSugar.Autofill(this, tx, null);
        }

        /// <inheritdoc />
        public Task<Submit> Submit(Dictionary<string, dynamic> tx, Wallet wallet)
        {
            return SubmitSugar.Submit(this, tx, true, false, wallet);
        }

        /// <inheritdoc />
        public Task<uint> GetLedgerIndex()
        {
            return GetLedgerSugar.GetLedgerIndex(this);
        }
        /// <inheritdoc />
        public Task<string> GetXrpBalance(string address)
        {
            return BalancesSugar.GetXrpBalance(this, address);
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
        /// <inheritdoc />
        public Task<AccountChannels> AccountChannels(AccountChannelsRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<AccountChannels> task = new TaskCompletionSource<AccountChannels>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request.Id;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(AccountChannels);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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



        /// <inheritdoc />
        public Task<LOLedger> Ledger(LedgerRequest request)
        {
            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<LOLedger> task = new TaskCompletionSource<LOLedger>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = Guid.Parse("1A3B944E-3632-467B-A53A-206305310BAE");
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(LOLedger);

            tasks.TryAdd(request.Id, taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public Task<ServerInfo> ServerInfo(ServerInfoRequest request)
        {
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public Task<Dictionary<string, dynamic>> Request(Dictionary<string, dynamic> request)
        {

            var command = JsonConvert.SerializeObject(request, serializerSettings);
            TaskCompletionSource<Dictionary<string, dynamic>> task = new TaskCompletionSource<Dictionary<string, dynamic>>();

            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = request["id"];
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(Dictionary<string, dynamic>);

            tasks.TryAdd(request["id"], taskInfo);

            client.SendMessage(command);
            return task.Task;
        }

        private static void Error(Exception ex, WebSocketClient client)
        {
            throw new Exception(ex.Message, ex);
        }

        private void MessageReceived(string s, WebSocketClient client)
        {
            if (string.IsNullOrWhiteSpace(s))
                return;

            var json = JObject.Parse(s);
            var can_get_type = json.TryGetValue("type", out var responseType);
            if (!can_get_type)
                throw new ArgumentNullException("type", "Unknown response type");
            Enum.TryParse(responseType.ToString(), out ResponseStreamType type);

            switch (type)
            {
                case ResponseStreamType.response:
                    {
                        OnResponse?.Invoke(s);
                        break;
                    }
                case ResponseStreamType.connected:
                    {
                        break;
                    }
                case ResponseStreamType.disconnected:
                    {
                        break;
                    }
                case ResponseStreamType.ledgerClosed:
                    {
                        var response = JsonConvert.DeserializeObject<LedgerStreamResponseResult>(s);
                        OnLedgerClosed?.Invoke(response);
                        break;
                    }
                case ResponseStreamType.validationReceived:
                    {
                        var response = JsonConvert.DeserializeObject<ValidationsStreamResponseResult>(s);
                        OnValidation?.Invoke(response);
                        break;
                    }
                case ResponseStreamType.transaction:
                    {
                        var response = JsonConvert.DeserializeObject<TransactionStreamResponseResult>(s);
                        OnTransaction?.Invoke(response);
                        break;
                    }
                case ResponseStreamType.peerStatusChange:
                    {
                        var response = JsonConvert.DeserializeObject<PeerStatusStreamResponseResult>(s);
                        OnPeerStatusChange?.Invoke(response);
                        break;
                    }
                case ResponseStreamType.consensusPhase:
                    {
                        var response = JsonConvert.DeserializeObject<ConsensusStreamResponseResult>(s);
                        OnConsensusPhase?.Invoke(response);
                        break;
                    }
                case ResponseStreamType.path_find:
                    {
                        var response = JsonConvert.DeserializeObject<PathFindStreamResult>(s);
                        OnPathFind?.Invoke(response);
                        break;
                    }
                case ResponseStreamType.error:
                    {
                        var response = JsonConvert.DeserializeObject<ErrorResponse>(s);
                        OnError?.Invoke(response);
                        break;
                    }
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void OnMessageReceived(string s)
        {
            var response = JsonConvert.DeserializeObject<RippleResponse>(s);

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

        #region IDisposable

        public void Dispose()
        {
            client?.Disconnect();
            client?.Dispose();
        }

        #endregion
    }
}
