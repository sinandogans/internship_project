using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using ServiceLayer.Utilities.MessageBroker.Abstract;

namespace ServiceLayer.Utilities.MessageBroker.RabbitMQ
{
    public class MqPublisherHelper : IPublisherHelper
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;

        public MqPublisherHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _factory = new ConnectionFactory() { HostName = _configuration["RabbitMq:Hostname"], UserName = _configuration["RabbitMq:Username"], Password = _configuration["RabbitMq:Password"] };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public void PublishMessage<T>(T message, string exchangeOrTopic, string? routingKey = null)
        {
            //_channel.QueueDeclare("q1", false, false, false, null);
            //_channel.QueueDeclare("q2", false, false, false, null);
            //_channel.QueueBind("q1", "e1", "r1");
            //_channel.QueueBind("q1", "e1", "r2");

            //var body = Encoding.UTF8.GetBytes(message.ToString());
            //_channel.BasicPublish(exchange: exchange, routingKey: routingKey, body: body);
        }
    }
}
