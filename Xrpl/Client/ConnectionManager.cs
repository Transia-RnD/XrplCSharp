using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/client/ConnectionManager.ts

namespace Xrpl.Client
{
    public class ConnectionManager
    {
        private List<(Action resolve, Action<Exception> reject)> promisesAwaitingConnection = new List<(Action resolve, Action<Exception> reject)>();

        public void ResolveAllAwaiting()
        {
            foreach (var (resolve, _) in promisesAwaitingConnection)
            {
                resolve();
            }
            promisesAwaitingConnection = new List<(Action resolve, Action<Exception> reject)>();
        }

        public void RejectAllAwaiting(Exception error)
        {
            foreach (var (_, reject) in promisesAwaitingConnection)
            {
                reject(error);
            }
            promisesAwaitingConnection = new List<(Action resolve, Action<Exception> reject)>();
        }

        public async Task AwaitConnection()
        {
            var tcs = new TaskCompletionSource<object>();
            promisesAwaitingConnection.Add((() => tcs.SetResult(null), (ex) => tcs.SetException(ex)));
            await tcs.Task;
        }
    }
}