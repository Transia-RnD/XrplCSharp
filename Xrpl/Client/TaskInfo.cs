using System;

namespace Xrpl.Client
{
    internal class TaskInfo
    {
        public Guid TaskId { get; set; }

        public Type Type { get; set; }

        public dynamic TaskCompletionResult { get; set; }

        public bool RemoveUponCompletion { get; set; }

        public TaskInfo()
        {
            RemoveUponCompletion = true;
        }
    }
    
}
