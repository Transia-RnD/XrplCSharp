using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/client/ConnectionManager.ts

namespace Xrpl.Client
{
    public class ConnectionManager
    {
        private List<(Action resolve, Action<Exception> reject)> PromisesAwaitingConnection = new List<(Action resolve, Action<Exception> reject)>();

        public void ResolveAllAwaiting()
        {
            foreach (var (resolve, _) in PromisesAwaitingConnection)
            {
                resolve();
            }

            PromisesAwaitingConnection = new List<(Action resolve, Action<Exception> reject)>();
        }

        public void RejectAllAwaiting(Exception error)
        {
            foreach (var (_, reject) in PromisesAwaitingConnection)
            {
                reject(error);
            }

            PromisesAwaitingConnection = new List<(Action resolve, Action<Exception> reject)>();
        }

        public async Task AwaitConnection()
        {
            var tcs = new TaskCompletionSource<object>();
            PromisesAwaitingConnection.Add((() => tcs.SetResult(null), (ex) => tcs.SetException(ex)));
            await tcs.Task;
        }
    }
}