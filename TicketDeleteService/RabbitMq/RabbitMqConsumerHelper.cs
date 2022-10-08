using Microsoft.Extensions.Configuration;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMq
{
    public class RabbitMqConsumerHelper
    {
        public ConnectionFactory _factory;
        public EventingBasicConsumer _consumer;
        public IConnection _connection;
        public IModel _channel;
        public IConfiguration _configuration;
        public ulong _deliveryTag;

        public RabbitMqConsumerHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Connect()
        {
            _factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMq:Hostname"],
                UserName = _configuration["RabbitMq:Username"],
                Password = _configuration["RabbitMq:Password"]
            };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _consumer = new EventingBasicConsumer(_channel);
        }

        //public T ConsumeMessage<T>(string queue)
        //{
        //    string? message = null;
        //    _consumer.Received += (model, ea) =>
        //    {
        //        var body = ea.Body.ToArray();
        //        message = Encoding.UTF8.GetString(body);
        //        _deliveryTag = ea.DeliveryTag;
        //        Console.WriteLine(message);
        //    };
        //    _channel.BasicQos(0, 1, false);
        //    _channel.BasicConsume(
        //        queue: queue,
        //        autoAck: false,
        //        consumer: _consumer);

        //    Thread.Sleep(1000);

        //    if (message == null)
        //    {
        //        return default;
        //    }
        //    return JsonSerializer.Deserialize<T>(message);

        //}

        public void AckMessage(bool multipleAck = false)
        {
            _channel.BasicAck(_deliveryTag, multipleAck);
        }

        public IModel GetChannel()
        {
            return _channel;
        }
        public EventingBasicConsumer GetConsumer()
        {
            return _consumer;
        }
    }
}
