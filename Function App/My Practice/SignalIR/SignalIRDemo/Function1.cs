using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace SignalIRDemo
{
    public static class Function1
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "EventHubDemoExample1")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }
    }
}