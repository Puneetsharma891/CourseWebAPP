using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace StorageQueueFunctionTrigger
{
    public static class StorageQueueFunction
    {
        [FunctionName("GetMessageFromQueue")]
        public static void Run([QueueTrigger("myqueue", Connection = "StorageQueueConnection")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
