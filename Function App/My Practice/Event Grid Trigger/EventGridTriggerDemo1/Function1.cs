// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using System.Net;
using System.Reflection.PortableExecutable;

namespace EventGridTriggerDemo1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
        }
    }
}



























//$resourceGroupName = "EventGridResourceGroup"
//$topicName = "eventgriddemofunctionstopic"
//$endpoint = (Get - AzEventGridTopic - ResourceGroupName $resourceGroupName - Name $topicName).Endpoint
//$keys = Get-AzEventGridTopicKey -ResourceGroupName $resourceGroupName -Name $topicName




//$eventId = Get - Random 99999
//$eventDate = Get - Date - Formate s

//$htbody = @{
//	id=$eventID
//	eventType="recordInserted"
//	subject="myapp/ehcles/motorcyles"
//	eventTime=$eventDate
//	data = @{
//	make="ducati"
//	model="Monster"
//	}
//	dataVersion = "1.0"
//}


//$body = "[" + (ConvertTo - Json $htbody)+"]"

//Invoke - WebRequest - Uri $endpoint - Method POST - Body $body - Headers @{ "aeg-sas-key" = $keys.Key1}

