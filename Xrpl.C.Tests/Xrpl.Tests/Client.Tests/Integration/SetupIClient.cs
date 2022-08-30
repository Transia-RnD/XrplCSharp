using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrpl.Client;
using Xrpl.Client.Models.Ledger;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/test/integration/setup.ts

namespace Xrpl.Tests.Client.Tests.Integration
{
    public class Integration
    {
        public static Task SetupIClient(string serverUrl)
        {
            //Wallet wallet = Wallet.generate();
            var promise = new TaskCompletionSource();
            IRippleClient client = new RippleClient(serverUrl);
            client.Connect();
            //FundAccount(client, wallet);
            return promise.Task;
        }
    }
}

//// var promise = new Promise<MyResult>;
//var promise = new TaskCompletionSource<MyResult>();

//// handlerMyEventsWithHandler(msg => promise.Complete(msg););
//handlerMyEventsWithHandler(msg => promise.TrySetResult(msg));

//// var myResult = promise.Future.Await(2000);
//var completed = await Task.WhenAny(promise.Task, Task.Delay(2000));
//if (completed == promise.Task)
//    ; // Do something on timeout
//var myResult = await completed;

//Assert.Equals("my header", myResult.Header);