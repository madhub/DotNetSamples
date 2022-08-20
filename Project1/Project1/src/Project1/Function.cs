using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using demo;
using Newtonsoft.Json;

QueueEvent queueEvent = new QueueEvent( )
        { EventType = "queue", MessageHeader = "Queue header", MessagePayload = "Queue payload" };

HttpEvent httpEvent = new HttpEvent()
{ EventType = "http", HttpMethod = "http headers", HttpPayload = "http body" };

var queueeventstr = JsonConvert.SerializeObject( queueEvent );
Console.WriteLine(queueeventstr);

var queueeventObj = JsonConvert.DeserializeObject<dynamic>(queueeventstr);
Function(queueeventObj);

var httpeventstr = JsonConvert.SerializeObject(httpEvent);
Console.WriteLine(httpeventstr);

var httpeventObj = JsonConvert.DeserializeObject<dynamic>(httpeventstr);
Function(httpeventObj);
Function(new BaseEvent() { EventType="Xyz"});
Function("demo");

Console.WriteLine("Done");
void Function(dynamic processEvent)
{
    
    Console.WriteLine(processEvent.EventType);
    if (processEvent.EventType == "queue")
    {

        Console.WriteLine($"Its Queue Event {processEvent.MessageHeader} - {processEvent.MessagePayload}");
    }
    else if (processEvent.EventType == "http")
    {
        Console.WriteLine($"Its Http Event {processEvent.HttpMethod}- {processEvent.HttpPayload}");
    }
    else
    {
        Console.WriteLine($"unknow event:{processEvent}");


    }

}