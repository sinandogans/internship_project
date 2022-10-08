using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ServiceLayer.Utilities.MessageBroker.RabbitMQ
{

    public class MqPublisherHelper
    {
        IConfiguration _configuration;
        RabbitMqOptions _rabbitMqOptions;

        public MqPublisherHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _rabbitMqOptions = _configuration.GetSection("RabbitMq").Get<RabbitMqOptions>();
        }

        public void Publish<T>(T message, string exchange, string routingKey)
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqOptions.HostName,
                UserName = _rabbitMqOptions.UserName,
                Password = _rabbitMqOptions.Password,
                Port = _rabbitMqOptions.Port
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("e1", "direct", true, false, null);
                channel.QueueDeclare("q1", true, false, false, null);
                channel.QueueDeclare("q2", true, false, false, null);
                channel.QueueBind("q1", "e1", "r1");
                channel.QueueBind("q2", "e1", "r2");

                var stringMessage = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(stringMessage);

                channel.BasicPublish(
                    exchange: exchange,
                    routingKey: routingKey,
                    body: body
                    );
            }
        }
    }
}
