namespace ServiceLayer.Utilities.MessageBroker.Abstract
{
    public interface IPublisherHelper
    {
        void PublishMessage<T>(T message, string exchangeOrTopic, string? routingKey = null);
    }
}
