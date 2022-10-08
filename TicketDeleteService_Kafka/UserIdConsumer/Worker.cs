using DataAccess.MongoDb.Abstract;
using Worker.MessageBroker.Kafka;

namespace UserIdConsumer
{
    public class Worker : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ITicketDal _ticketDal;
        private readonly KafkaConsumerHelper _consumerHelper;
        private readonly int _maxMessage = 10;
        private readonly int _messageWait = 10;
        private readonly int _maxEmpty = 2;
        private readonly int _emptyWait = 20;
        private readonly string _topic = "deleteduserid";
        private readonly string _groupId = "G14";

        public Worker(IHostApplicationLifetime hostApplicationLifetime, ITicketDal ticketDal, KafkaConsumerHelper consumerHelper)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _ticketDal = ticketDal;
            _consumerHelper = consumerHelper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int messageCount = 0;
            int emptyCount = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                _consumerHelper.Connect(_groupId);
                var userId = _consumerHelper.ConsumeMessage<int>(_topic);

                if (userId != default)
                {
                    //var ticketsToDelete = _ticketDal.GetAllByUserId(userId);
                    //foreach (var ticket in ticketsToDelete)
                    //    _ticketDal.Delete(ticket.Id);
                    messageCount++;
                    emptyCount = 0;
                    Console.WriteLine(userId);
                    _consumerHelper.Commit();
                    Thread.Sleep(TimeSpan.FromSeconds(_messageWait));
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(_emptyWait));
                    emptyCount++;
                }
                if (messageCount == _maxMessage || emptyCount == _maxEmpty)
                {
                    _hostApplicationLifetime.StopApplication();
                }
            }
        }
    }
}