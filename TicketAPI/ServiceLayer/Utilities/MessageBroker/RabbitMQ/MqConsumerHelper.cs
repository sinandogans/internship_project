using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

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
            _factory = new ConnectionFactory() { HostName = _configuration["RabbitMq:Hostname"], UserName = _configuration["RabbitMq:Username"], Password = _configuration["RabbitMq:Password"] };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _consumer = new EventingBasicConsumer(_channel);
        }

        public Guid ConsumeMessage(string queue)
        {
            Guid message = Guid.Empty;
            var data = _channel.BasicGet(queue, true);
            if (data == null)
                return message;

            var body = data.Body.ToArray();
            message = new Guid(Encoding.UTF8.GetString(body));
            return message;
        }
    }
}
