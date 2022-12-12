//See https://aka.ms/new-console-template for more information

using System.Net.WebSockets;
using Newtonsoft.Json;
using Xrpl.Client;
using Xrpl.Models.Methods;

Console.WriteLine("Hello, World!");

//var server = "wss://xrplcluster.com/";

//var client = new XrplClient(server);

//client.OnConnected += () =>
//{
//    Console.WriteLine("CONNECTED");
//    return Task.CompletedTask;
//};

//client.OnDisconnect += (code) =>
//{
//    Console.WriteLine($"DISCONECTED CODE: {code}");
//    Console.WriteLine("DISCONECTED");
//    return Task.CompletedTask;
//};

//client.OnError += (errorCode, errorMessage, error, data) =>
//{
//    Console.WriteLine(errorCode);
//    Console.WriteLine(errorMessage);
//    Console.WriteLine(data);
//    return Task.CompletedTask;
//};

//client.OnTransaction += Response =>
//{
//    Console.WriteLine(Response.Transaction.TransactionType.ToString());
//    return Task.CompletedTask;
//};

//client.OnLedgerClosed += r =>
//{
//    Console.WriteLine("CALLBACK");
//    Console.WriteLine(r);
//    return Task.CompletedTask;
//};

//await client.Connect();


//var subscribe = await client.Subscribe(
//new SubscribeRequest()
//{
//    Streams = new List<string>(new[]
//    {
//        "ledger",
//    })
//});

//Console.ReadLine();


//var server = "wss://xrplcluster.com/";

//var client = new WebSocketClient(server);

//client.OnConnected += async (t) =>
//{
//    Console.WriteLine("CONNECTED");
//    var request = new SubscribeRequest()
//    {
//        Streams = new List<string>(new[]
//        {
//        "ledger",
//    })
//    };
//    var serializerSettings = new JsonSerializerSettings();
//    serializerSettings.NullValueHandling = NullValueHandling.Ignore;
//    serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
//    serializerSettings.FloatParseHandling = FloatParseHandling.Double;
//    serializerSettings.FloatFormatHandling = FloatFormatHandling.DefaultValue;
//    string jsonString = JsonConvert.SerializeObject(request, serializerSettings);
//    await client.Send(jsonString);
//};

//client.OnMessageReceived += (message) =>
//{
//    Console.WriteLine(message);
//};

//await client.ConnectAsync();

