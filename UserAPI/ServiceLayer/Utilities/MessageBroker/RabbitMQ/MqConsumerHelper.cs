using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ServiceLayer.Utilities.MessageBroker.RabbitMQ
{
    public class MqConsumerHelper
    {
        private readonly ConnectionFactory _factory;
        private readonly EventingBasicConsumer _consumer;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IConfiguration _configuration;
        public MqConsumerHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _factory = new ConnectionFactory() { HostName = _configuration["RabbitMq:Hostname"], UserName = _configuration["RabbitMq:Username"], Password = _configuration["RabbitMq:Password"]};
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _consumer = new EventingBasicConsumer(_channel);

        }

        //public string ConsumeMessage(string exchange, string routingKey)
        //{
        //    string message = string.Empty;
        //    _consumer.Received += (model, ea) =>
        //    {
        //        var body = ea.Body.ToArray();
        //        message = Encoding.UTF8.GetString(body);
        //    };

        //    _channel.BasicConsume(queue: "q1", true, consumer: _consumer);

        //    return message;
        //}
    }
}
