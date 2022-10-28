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
using System.Timers;
using System.Threading;
using Timer = System.Timers.Timer;
using Org.BouncyCastle.Asn1.Ocsp;

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
            public int Id { get; set; }
            public string Message { get; set; }
            public Task<dynamic> Promise { get; set; }
        }

        private Timer timer;
        private int nextId = 0;
        private readonly ConcurrentDictionary<int, TaskInfo> promisesAwaitingResponse = new ConcurrentDictionary<int, TaskInfo>();
        private readonly JsonSerializerSettings serializerSettings;

        public RequestManager()
        {
            serializerSettings = new JsonSerializerSettings();
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        }

        /// <summary>
        /// </summary>
        public void Resolve(int id, BaseResponse response)
        {
            var promise = promisesAwaitingResponse.TryGetValue(id, out var taskInfo);
            if (taskInfo == null)
            {
                throw new XrplError($"No existing promise with id {id}");
            }
            // todo: should stop task timout if need to
            //clearTimeout(promise)
            timer.Stop();
            var deserialized = JsonConvert.DeserializeObject(response.Result.ToString(), taskInfo.Type, serializerSettings);
            var setResult = taskInfo.TaskCompletionResult.GetType().GetMethod("SetResult");
            setResult.Invoke(taskInfo.TaskCompletionResult, new[] { deserialized });
            this.DeletePromise(id, taskInfo);
        }

        /// <summary>
        /// </summary>
        public void Reject(int id, Exception error)
        {
            var promise = promisesAwaitingResponse.TryGetValue(id, out var taskInfo);
            if (taskInfo == null)
            {
                throw new XrplError($"No existing promise with id {id}");
            }
            // todo: should stop task timout if need to
            //clearTimeout(promise)
            timer.Stop();
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

        public XrplRequest CreateRRequest<R>(R request, int timeout)
        {
            var id = request.TryGetValue("id", out var newId);
            if (!id)
            {
                newId = this.nextId;
                this.nextId += 1;
            }
            else
            {
                newId = request["id"];
            }

            request["id"] = newId;

            string newRequest = JsonConvert.SerializeObject(request, serializerSettings);

            if (this.promisesAwaitingResponse.ContainsKey(newId))
            {
                throw new XrplError($"Response with id '${newId}' is already pending");
            }

            TaskCompletionSource<object> task = new TaskCompletionSource<object>();
            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = newId;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(object);

            promisesAwaitingResponse.TryAdd(newId, taskInfo);

            timer = new Timer(timeout);
            //timer.Elapsed += (sender, e) => task.SetException(new TimeoutError());
            timer.Elapsed += (sender, e) => this.Reject(newId, new TimeoutError());
            timer.Start();

            return new XrplRequest()
            {
                Id = newId,
                Message = newRequest,
                Promise = task.Task
            };
        }

        /// <summary>
        /// </summary>
        //public XrplRequest CreateRequest<R>(R request, int timeout)
        public XrplRequest CreateRequest(Dictionary<string, dynamic> request, int timeout)
        {
            var id = request.TryGetValue("id", out var newId);
            if (!id)
            {
                newId = this.nextId;
                this.nextId += 1;
            }
            else
            {
                newId = request["id"];
            }

            request["id"] = newId;

            string newRequest = JsonConvert.SerializeObject(request, serializerSettings);

            if (this.promisesAwaitingResponse.ContainsKey(newId))
            {
                throw new XrplError($"Response with id '${newId}' is already pending");
            }

            TaskCompletionSource<object> task = new TaskCompletionSource<object>();
            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = newId;
            taskInfo.TaskCompletionResult = task;
            taskInfo.Type = typeof(object);

            promisesAwaitingResponse.TryAdd(newId, taskInfo);

            timer = new Timer(timeout);
            //timer.Elapsed += (sender, e) => task.SetException(new TimeoutError());
            timer.Elapsed += (sender, e) => this.Reject(newId, new TimeoutError());
            timer.Start();

            return new XrplRequest()
            {
                Id = newId,
                Message = newRequest,
                Promise = task.Task
            };
        }

        public void HandleResponse(dynamic response)
        {
            Console.WriteLine(response);
            string id = response.TryGetValue("id", out string tmp);
            if (id == null)
            {
                throw new XrplError("Valid id not found in response");
            }

            if (!promisesAwaitingResponse.ContainsKey(response["id"]))
            {
                return;
            }

            if (response["status"] == null)
            {
                ResponseFormatError error = new ResponseFormatError("Response has no status");
                this.Reject(response.id, error);
            }

            if (response["status"] == "error")
            {

            }

            if (response["status"] != "success")
            {

            }

            //var taskInfoResult = promisesAwaitingResponse.TryGetValue(response.Id, out TaskInfo taskInfo);
            //if (taskInfoResult == false) throw new Exception("Task not found");

            //var deserialized = JsonConvert.DeserializeObject(response.Result.ToString(), taskInfo.Type, serializerSettings);
            //var setResult = taskInfo.TaskCompletionResult.GetType().GetMethod("SetResult");
            //setResult.Invoke(taskInfo.TaskCompletionResult, new[] { deserialized });
            this.Resolve(response["id"], response);
        }

        /// <summary>
        /// </summary>
        public void DeletePromise(int id, TaskInfo taskInfo)
        {
            this.promisesAwaitingResponse.TryRemove(id, out taskInfo);
        }
    }
}

