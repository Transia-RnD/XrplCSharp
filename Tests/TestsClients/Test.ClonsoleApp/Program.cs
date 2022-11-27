//See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using System;

using Xrpl.Client;
using Xrpl.Models.Methods;

Console.WriteLine("Hello, World!");

var server = "wss://xrplcluster.com/";

var client = new XrplClient(server);

client.Connect();
var account_response = await client.AccountChannels(new AccountChannelsRequest("rUtssZDe6QgC3GLxuU6EC1tQ8nYBb5F87b"){Limit = 400});
var channels = account_response.Channels;
while (account_response.Marker is not null)
{
    account_response = await client.AccountChannels(new AccountChannelsRequest("rUtssZDe6QgC3GLxuU6EC1tQ8nYBb5F87b") { Limit = 50 ,Marker = account_response.Marker});
    if (account_response.Channels.Count > 0)
        channels.AddRange(account_response.Channels);
}

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



