using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xrpl.Client.Exceptions;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/client/ConnectionManager.ts

namespace Xrpl.Client
{
    public class ConnectionManager
    {
        private struct VoidResult { }

        private List<TaskCompletionSource<VoidResult>> PromisesAwaitingConnection = new List<TaskCompletionSource<VoidResult>>();

        public void ResolveAllAwaiting()
        {
            this.PromisesAwaitingConnection.ForEach((p) =>
            {
                p.TrySetResult(default(VoidResult));
            });
            this.PromisesAwaitingConnection = new List<TaskCompletionSource<VoidResult>>();
        }

        public void RejectAllAwaiting(Exception error)
        {
            this.PromisesAwaitingConnection.ForEach((p) =>
            {
                p.TrySetException(error);
            });
            this.PromisesAwaitingConnection = new List<TaskCompletionSource<VoidResult>>();
        }

        public Task AwaitConnection()
        {
            TaskCompletionSource<VoidResult> task = new TaskCompletionSource<VoidResult>();
            this.PromisesAwaitingConnection.Add(task);
            return task.Task;
        }
    }
}

