using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/client/ConnectionManager.ts

namespace Xrpl.Client
{
    public class ConnectionManager
    {
        private List<TaskCompletionSource> PromisesAwaitingConnection = new List<TaskCompletionSource>();

        public void ResolveAllAwaiting()
        {
            this.PromisesAwaitingConnection.ForEach((p) =>
            {
                p.TrySetResult();
            });
            this.PromisesAwaitingConnection = new List<TaskCompletionSource>();
        }

        public void RejectAllAwaiting(Exception error)
        {
            this.PromisesAwaitingConnection.ForEach((p) =>
            {
                p.TrySetException(error);
            });
            this.PromisesAwaitingConnection = new List<TaskCompletionSource>();
        }

        public Task AwaitConnection()
        {
            TaskCompletionSource task = new TaskCompletionSource();
            this.PromisesAwaitingConnection.Add(task);
            return task.Task;
        }
    }
}

