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
using System.Reflection;
using System.Xml.Linq;

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
            public Task<Dictionary<string, dynamic>> Promise { get; set; }
        }

        public class XrplGRequest
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
            foreach (var id in this.promisesAwaitingResponse.Keys)
            {
                this.Reject(id, error);
                this.DeletePromise(id, null);
            }
        }

        public XrplGRequest CreateGRequest<T, R>(R request, int timeout)
        {
            int newId = 0;
            var info = request.GetType().GetProperty("Id");
            if (info.GetValue(request) == null)
            {
                newId = this.nextId;
                this.nextId += 1;
            }
            else
            {
                newId = (int)info.GetValue(request);
            }

            info.SetValue(request, newId, null);

            string newRequest = JsonConvert.SerializeObject(request, serializerSettings);

            if (this.promisesAwaitingResponse.ContainsKey(newId))
            {
                throw new XrplError($"Response with id '${newId}' is already pending");
            }

            TaskCompletionSource<dynamic> task = new TaskCompletionSource<dynamic>();
            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = newId;
            taskInfo.TaskCompletionResult = task;
            var typeInfo = request.GetType().GetProperty("Command");
            taskInfo.RemoveUponCompletion = (string)typeInfo.GetValue(request) == "subscribe" ? false : true;
            taskInfo.Type = typeof(T);

            promisesAwaitingResponse.TryAdd(newId, taskInfo);

            timer = new Timer(timeout);
            timer.Elapsed += (sender, e) => this.Reject(newId, new TimeoutError());
            timer.Start();

            return new XrplGRequest()
            {
                Id = newId,
                Message = newRequest,
                Promise = task.Task
            };
        }

        /// <summary>
        /// </summary>
        public XrplRequest CreateRequest(Dictionary<string, dynamic> request, int timeout)
        {
            int newId;
            var _id = request.TryGetValue("id", out var id);
            if (!_id)
            {
                newId = this.nextId;
                this.nextId += 1;
            }
            else
            {
                newId = (int)id;
            }

            request["id"] = newId;

            string newRequest = JsonConvert.SerializeObject(request, serializerSettings);
            Console.WriteLine($"CLIENT SEND: {newRequest}");

            if (this.promisesAwaitingResponse.ContainsKey(newId))
            {
                throw new XrplError($"Response with id '${newId}' is already pending");
            }

            TaskCompletionSource<Dictionary<string, dynamic>> task = new TaskCompletionSource<Dictionary<string, dynamic>>();
            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = newId;
            taskInfo.TaskCompletionResult = task;
            var typeInfo = request.GetType().GetProperty("Command");
            taskInfo.RemoveUponCompletion = (string)typeInfo.GetValue(request) == "subscribe" ? false : true;
            taskInfo.Type = typeof(Dictionary<string, dynamic>);

            promisesAwaitingResponse.TryAdd(newId, taskInfo);

            timer = new Timer(timeout);
            timer.Elapsed += (sender, e) => this.Reject(newId, new TimeoutError());
            timer.Start();

            return new XrplRequest()
            {
                Id = newId,
                Message = newRequest,
                Promise = task.Task
            };
        }

        public void HandleResponse(BaseResponse response)
        {
            if (response.Id == null)
            {
                throw new XrplError("Valid id not found in response");
            }

            if (!promisesAwaitingResponse.ContainsKey(response.Id))
            {
                Console.WriteLine("Valid id not found in promises");
                return;
            }

            if (response.Status == null)
            {
                ResponseFormatError error = new ResponseFormatError("Response has no status");
                this.Reject(response.Id, error);
                //return;
            }

            if (response.Status == "error")
            {
                XrplError error = new XrplError(response.ErrorMessage ?? response.Error);
                this.Reject(response.Id, error);
                //return;
            }

            if (response.Status != "success")
            {
                XrplError error = new XrplError($"unrecognized response.status: ${response.Status ?? ""}");
                this.Reject(response.Id, error);
                //return;
            }

            this.Resolve(response.Id, response);
        }

        /// <summary>
        /// </summary>
        public void DeletePromise(int id, TaskInfo taskInfo)
        {
            this.promisesAwaitingResponse.TryRemove(id, out taskInfo);
        }
    }
}

