//See https://aka.ms/new-console-template for more information

using System.Net.WebSockets;
using Newtonsoft.Json;
using Xrpl.Client;
using Xrpl.Models.Methods;

//Console.WriteLine("Hello, World!");

//var server = "wss://xrplcluster.com/";

//var client = new XrplClient(server);

//client.OnConnected += () =>
//{
//    Console.WriteLine("CONNECTED");
//};

//client.OnDisconnect += (code) =>
//{
//    Console.WriteLine($"DISCONECTED CODE: {code}");
//    Console.WriteLine("DISCONECTED");
//};

//client.OnError += (errorCode, errorMessage, error, data) =>
//{
//    Console.WriteLine(errorCode);
//    Console.WriteLine(errorMessage);
//    Console.WriteLine(data);
//};

//client.OnTransaction += Response =>
//{
//    Console.WriteLine(Response.Transaction.TransactionType.ToString());
//};

//client.OnLedgerClosed += r =>
//{
//    Console.WriteLine("CALLBACK");
//    Console.WriteLine(r);
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
////Console.WriteLine(subscribe);

////while (client.connection.State() == WebSocketState.Open)
////{
////    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
////}

//System.Threading.Thread.Sleep(TimeSpan.FromSeconds(8));

//await client.Disconnect();

////Console.ReadLine();


var server = "wss://xrplcluster.com/";

var client = new WebSocketClient(server);

await client.ConnectAsync();

bool isFinished = false;
client.OnMessageReceived += (ws, message) =>
{
    Console.WriteLine(message);
    isFinished = true;
};

var request = new SubscribeRequest()
{
    Streams = new List<string>(new[]
        {
        "ledger",
    })
};
var serializerSettings = new JsonSerializerSettings();
serializerSettings.NullValueHandling = NullValueHandling.Ignore;
serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
serializerSettings.FloatParseHandling = FloatParseHandling.Double;
serializerSettings.FloatFormatHandling = FloatFormatHandling.DefaultValue;
string jsonString = JsonConvert.SerializeObject(request, serializerSettings);
await client.SendMessageAsync(jsonString);

while (!isFinished)
{
    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
}


