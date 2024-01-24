namespace ProductMicroservice.AsyncDataServices.Interfaces
{
    public interface IMessageBusClient
    {
        void PublishMessage<T>(T message, string routingKey);
    }
}