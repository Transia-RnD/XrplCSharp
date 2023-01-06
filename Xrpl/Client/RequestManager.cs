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
using Org.BouncyCastle.Utilities;
using TimeoutException = Xrpl.Client.Exceptions.TimeoutException;

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
            public Task<Dictionary<string, dynamic>> Promise { get; set; }
        }

        public class XrplGRequest
        {
            public Guid Id { get; set; }
            public string Message { get; set; }
            public Task<dynamic> Promise { get; set; }
        }

        private Guid nextId = Guid.NewGuid();
        private readonly ConcurrentDictionary<Guid, Timer> timeoutsAwaitingResponse = new ConcurrentDictionary<Guid, Timer>();
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
                throw new XrplException($"No existing promise with id {id}");
            }
            var hasTimer = this.timeoutsAwaitingResponse.TryRemove(id, out var timer);
            if (hasTimer)
                timer.Stop();

            var deserialized = JsonConvert.DeserializeObject(response.Result.ToString(), taskInfo.Type, serializerSettings);
            var setResult = taskInfo.TaskCompletionResult.GetType().GetMethod("TrySetResult");
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
                throw new XrplException($"No existing promise with id {id}");
            }
            // todo: should stop task timout if need to
            var hasTimer = this.timeoutsAwaitingResponse.TryRemove(id, out var timer);
            if (hasTimer)
                timer.Stop();
            var setException = taskInfo.TaskCompletionResult.GetType().GetMethod("TrySetException", new Type[] { typeof(Exception) }, null);
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
            Guid newId;
            var info = request.GetType().GetProperty("Id");
            if (info.GetValue(request) == null)
            {
                newId = this.nextId;
                this.nextId = Guid.NewGuid();
            }
            else
            {
                newId = (Guid)info.GetValue(request);
            }

            info.SetValue(request, newId, null);

            string newRequest = JsonConvert.SerializeObject(request, serializerSettings);

            if (this.promisesAwaitingResponse.ContainsKey(newId))
            {
                throw new XrplException($"Response with id '${newId}' is already pending");
            }

            TaskCompletionSource<dynamic> task = new TaskCompletionSource<dynamic>();
            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = newId;
            taskInfo.TaskCompletionResult = task;
            taskInfo.RemoveUponCompletion = true;
            taskInfo.Type = typeof(T);

            promisesAwaitingResponse.TryAdd(newId, taskInfo);

            Timer timer = new Timer(timeout);
            timer.Elapsed += (sender, e) => this.Reject(newId, new TimeoutException());
            timer.Start();
            timeoutsAwaitingResponse.TryAdd(newId, timer);

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
            Guid newId;
            var _id = request.TryGetValue("id", out var id);
            if (!_id)
            {
                newId = this.nextId;
                this.nextId = Guid.NewGuid();
            }
            else
            {
                newId = (Guid)id;
            }

            request["id"] = newId;

            string newRequest = JsonConvert.SerializeObject(request, serializerSettings);

            if (this.promisesAwaitingResponse.ContainsKey(newId))
            {
                throw new XrplException($"Response with id '${newId}' is already pending");
            }

            TaskCompletionSource<Dictionary<string, dynamic>> task = new TaskCompletionSource<Dictionary<string, dynamic>>();
            TaskInfo taskInfo = new TaskInfo();
            taskInfo.TaskId = newId;
            taskInfo.TaskCompletionResult = task;
            taskInfo.RemoveUponCompletion = true;
            taskInfo.Type = typeof(Dictionary<string, dynamic>);

            promisesAwaitingResponse.TryAdd(newId, taskInfo);

            Timer timer = new Timer(timeout);
            timer.Elapsed += (sender, e) => this.Reject(newId, new TimeoutException());
            timer.Start();
            timeoutsAwaitingResponse.TryAdd(newId, timer);

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
                throw new XrplException("Valid id not found in response");
            }

            if (!promisesAwaitingResponse.ContainsKey((Guid)response.Id))
            {
                Debug.WriteLine("Valid id not found in promises");
                return;
            }

            if (response.Status == null)
            {
                ResponseFormatException error = new ResponseFormatException("Response has no status");
                this.Reject((Guid)response.Id, error);
            }

            if (response.Status == "error")
            {
                XrplException error = new XrplException(response.ErrorMessage ?? response.Error);
                this.Reject((Guid)response.Id, error);
                return;
            }

            if (response.Status != "success")
            {
                XrplException error = new XrplException($"unrecognized response.status: ${response.Status ?? ""}");
                this.Reject((Guid)response.Id, error);
                return;
            }
            this.Resolve((Guid)response.Id, response);
        }

        /// <summary>
        /// </summary>
        public void DeletePromise(Guid id, TaskInfo taskInfo)
        {
            this.promisesAwaitingResponse.TryRemove(id, out taskInfo);
        }
    }
}