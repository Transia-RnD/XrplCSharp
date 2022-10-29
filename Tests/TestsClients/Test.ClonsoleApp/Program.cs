//See https://aka.ms/new-console-template for more information

using Xrpl.Client;
using Xrpl.Models.Methods;

Console.WriteLine("Hello, World!");

var server = "wss://xrplcluster.com/";

var client = new XrplClient(server);

client.OnConnected += () =>
{
    Console.WriteLine("CONNECTED");
};

client.OnDisconnect += (code) =>
{
    Console.WriteLine($"DISCONECTED CODE: {code}");
    Console.WriteLine("DISCONECTED");
};

client.OnError += (errorCode, errorMessage, error, data) =>
{
    Console.WriteLine(errorCode);
    Console.WriteLine(errorMessage);
    Console.WriteLine(data);
};

client.OnTransaction += Response =>
{
    Console.WriteLine(Response.Transaction.TransactionType.ToString());
};

client.Connect().Wait();
var subscribe = await client.Subscribe(
new SubscribeRequest()
{
    Streams = new List<string>(new[]
    {
        "transactions",
    })
});
Console.WriteLine(subscribe);

//System.Threading.Thread.Sleep(TimeSpan.FromSeconds(13));

//client.Disconnect();

//Console.ReadLine();



