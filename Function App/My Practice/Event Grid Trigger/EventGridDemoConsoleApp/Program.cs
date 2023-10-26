using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string topicEndpoint = "eventgriddemofunctionstopic";
        string topicKey = "m1PMV9BJ7kd0e0nXwnNIdaNqmpRfLHD0vGAZiCXKfhw=";

        TopicCredentials topicCredentials = new TopicCredentials(topicKey);
        EventGridClient client = new EventGridClient(topicCredentials);

        string eventType = "MyApp.CustomEventType";
        Uri subject = new Uri("https://eventgridfunctionappdemo.azurewebsites.net/runtime/webhooks/EventGrid?functionName=EventGridTrigger2&code=LfB1XGkcdQ9Wq1OsoENHXF800CsQXGLG3gbHOFchIqS-AzFuv006sA==");

        var events = new List<EventGridEvent>
        {
            new EventGridEvent
            {
                Id = Guid.NewGuid().ToString(),
                EventType = eventType,
                Data = new { message = "Event from C# application" },
                EventTime = DateTime.Now,
                Subject = subject.ToString(),
                DataVersion = "1.0",
            }
        };

        await client.PublishEventsAsync(topicEndpoint, events);
        Console.WriteLine("Events published successfully!");

    }
}
