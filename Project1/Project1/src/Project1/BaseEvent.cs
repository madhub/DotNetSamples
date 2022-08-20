namespace demo;
//// The function handler that will be called for each Lambda event
//var handler = (string input, ILambdaContext context) =>
//{
//    return input.ToUpper();
//};

//// Build the Lambda runtime client passing in the handler to call for each
//// event and the JSON serializer to use for translating Lambda JSON documents
//// to .NET types.
//await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
//        .Build()
//        .RunAsync();

public class BaseEvent
{
    public String EventType { get; set; }
}

public class HttpEvent : BaseEvent
{
    public String HttpMethod { get; set; }
    public String HttpPayload { get; set; }
}

public class QueueEvent : BaseEvent
{
    public String MessageHeader { get; set; }
    public String MessagePayload { get; set; }
}

