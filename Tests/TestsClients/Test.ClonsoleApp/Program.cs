// See https://aka.ms/new-console-template for more information

using Xrpl.ClientLib;
using Xrpl.Models.Methods;

Console.WriteLine("Hello, World!");

var server = "wss://xrplcluster.com/";

var client = new Client(server);

client.Connect();
var subscribe = await client.Subscribe(
    new SubscribeRequest()
    {
        Streams = new List<string>(new[]
        {
            "transactions",
        })
    });
Console.WriteLine(subscribe);

client.OnTransaction += Response =>
{
    Console.WriteLine(Response.Transaction.TransactionType.ToString());
};

Console.ReadLine();



