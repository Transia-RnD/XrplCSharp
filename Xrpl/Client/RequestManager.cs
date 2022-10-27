using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Tsp;
using Xrpl.Client.Exceptions;
using Xrpl.Models.Subscriptions;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Xrpl.BinaryCodec;

// https://github.com/XRPLF/xrpl.js/blob/main/packages/xrpl/src/client/RequestManager.ts

namespace Xrpl.Client
{
    /// <summary>
    /// Manage all the requests made to the websocket, and their async responses
    /// that come in from the WebSocket.Responses come in over the WS connection
    /// after-the-fact, so this manager will tie that response to resolve the
    /// original request.
    /// </summary>
    public class RequestManager
    {

        public class XrplRequest
        {
            public Guid Id { get; set; }
            public string Message { get; set; }
            public TaskInfo Promise { get; set; }
        }

        private Guid nextId = Guid.NewGuid();
        private readonly ConcurrentDictionary<Guid, TaskInfo> promisesAwaitingResponse = new ConcurrentDictionary<Guid, TaskInfo>();
        private readonly JsonSerializerSettings serializerSettings;

        public RequestManager()
        {
            serializerSettings = new JsonSerializerSettings();
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        }

        /// <summary>
        /// </summary>
        public void Resolve(Guid id, BaseResponse response)
        {
            var promise = promisesAwaitingResponse.TryGetValue(id, out var taskInfo);
            if (taskInfo == null)
            {
                throw new XrplError($"No existing promise with id {id}");
            }
            // todo: should stop task timout if need to
            //clearTimeout(promise)
            var deserialized = JsonConvert.DeserializeObject(response.Result.ToString(), taskInfo.Type, serializerSettings);
            var setResult = taskInfo.TaskCompletionResult.GetType().GetMethod("SetResult");
            setResult.Invoke(taskInfo.TaskCompletionResult, new[] { deserialized });
            this.DeletePromise(id, taskInfo);
        }

        /// <summary>
        /// </summary>
        public void Reject(Guid id, Exception error)
        {
            var promise = promisesAwaitingResponse.TryGetValue(id, out var taskInfo);
            if (taskInfo == null)
            {
                throw new XrplError($"No existing promise with id {id}");
            }
            // todo: should stop task timout if need to
            //clearTimeout(promise)
            var setException = taskInfo.TaskCompletionResult.GetType().GetMethod("SetException", new Type[] { typeof(Exception) }, null);
            setException.Invoke(taskInfo.TaskCompletionResult, new[] { error });
            this.DeletePromise(id, taskInfo);
        }

        /// <summary>
        /// </summary>
        public void RejectAll(Exception error)
        {
            foreach (var guid in this.promisesAwaitingResponse.Keys)
            {
                this.DeletePromise(guid, null);
            }
        }

        /// <summary>
        /// </summary>
        public XrplRequest CreateRequest(Dictionary<string, dynamic> request, int timeout)
        {
            var id = request.TryGetValue("id", out var newId);
            if (!id)
            {
                newId = this.nextId;
                this.nextId = Guid.NewGuid();
            }
            else
            {
                newId = request["id"];
            }

            string newRequest = JsonConvert.SerializeObject(request, serializerSettings);

            //const timer = setTimeout(
            //    () => this.reject(newId, new TimeoutError()),
            //    timeout,
            //)

            if (this.promisesAwaitingResponse.ContainsKey((Guid)newId))
            {
                throw new XrplError($"Response with id '${newId}' is already pending");
            }

            TaskCompletionSource<Dictionary<string, dynamic>> task = new TaskCompletionSource<Dictionary<string, dynamic>>();
            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = newId;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(Dictionary<string, dynamic>);

            promisesAwaitingResponse.TryAdd(newId, taskInfo);

            return new XrplRequest()
            {
                Id = (Guid)newId,
                Message = newRequest,
                Promise = taskInfo
            };
        }



        /// <summary>
        /// </summary>
        public void DeletePromise(Guid id, TaskInfo taskInfo)
        {
            this.promisesAwaitingResponse.TryRemove(id, out taskInfo);
        }
    }
}

