using DataAccess.MongoDb.Abstract;
using RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace UserIdConsumer
{
    public class Worker : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ITicketDal _ticketDal;
        private readonly RabbitMqConsumerHelper _consumerHelper;
        private readonly Stopwatch _stopwatch;

        private readonly TimeSpan _emptyWait = TimeSpan.FromSeconds(10);
        private int _messageCount = 0;
        private readonly int _maxMessageCount = 10;
        private readonly int _messageWait = 5;
        private readonly string _queueName = "q1";

        public Worker(ITicketDal ticketDal, RabbitMqConsumerHelper consumerHelper, IHostApplicationLifetime hostApplicationLifetime)
        {
            _ticketDal = ticketDal;
            _consumerHelper = consumerHelper;
            _hostApplicationLifetime = hostApplicationLifetime;
            _stopwatch = Stopwatch.StartNew();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumerHelper.Connect();
            _consumerHelper._consumer.Received += OnMessageRecieved;
            _consumerHelper._channel.BasicQos(0, 1, false);
            _consumerHelper._channel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                consumer: _consumerHelper._consumer);
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_stopwatch.Elapsed >= _emptyWait)
                {
                    _hostApplicationLifetime.StopApplication();
                }
            }
        }

        private void OnMessageRecieved(object sender, BasicDeliverEventArgs ea)
        {
            if (_messageCount == _maxMessageCount)
            {
                _hostApplicationLifetime.StopApplication();
            }
            _stopwatch.Stop();
            _stopwatch.Reset();

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var userId = JsonSerializer.Deserialize<int>(message);
            Console.WriteLine(userId);

            //var ticketsToDelete = _ticketDal.GetAllByUserId(userId);
            //foreach (var ticket in ticketsToDelete)
            //    _ticketDal.Delete(ticket.Id);
            //_consumerHelper.AckMessage();
            _consumerHelper._channel.BasicAck(ea.DeliveryTag, false);
            _messageCount++;
            Thread.Sleep(TimeSpan.FromSeconds(_messageWait));
            _stopwatch.Start();
        }
    }
}