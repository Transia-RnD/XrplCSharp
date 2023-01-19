////See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Net.WebSockets;
using Newtonsoft.Json;
using Xrpl.Client;
using Xrpl.Models.Methods;

Console.WriteLine("Hello, World!");

var server = "wss://xrplcluster.com/";

var client = new XrplClient(server);

client.OnConnected += async () =>
{
    Console.WriteLine("CONNECTED");
    var subscribe = await client.Subscribe(
    new SubscribeRequest()
    {
        Streams = new List<string>(new[]
        {
            "ledger",
        })
    });
};

client.OnDisconnect += (code) =>
{
    Console.WriteLine($"DISCONECTED CODE: {code}");
    Console.WriteLine("DISCONECTED");
    return Task.CompletedTask;
};

client.OnError += (errorCode, errorMessage, error, data) =>
{
    Console.WriteLine(errorCode);
    Console.WriteLine(errorMessage);
    Console.WriteLine(data);
    return Task.CompletedTask;
};

client.OnTransaction += Response =>
{
    Console.WriteLine(Response.Transaction.TransactionType.ToString());
    return Task.CompletedTask;
};

client.OnLedgerClosed += r =>
{
    Console.WriteLine("CALLBACK");
    Console.WriteLine(r);
    return Task.CompletedTask;
};

await client.Connect();

//_ = client.Connect();

Console.ReadLine();
