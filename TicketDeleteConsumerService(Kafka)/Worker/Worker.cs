using DataAccess.MongoDb.Abstract;
using Worker.MessageBroker.Kafka;

namespace Worker
{
    public class Worker : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ITicketDal _ticketDal;
        private readonly KafkaConsumerHelper _consumerHelper;
        private readonly int _maxMessage = 10;
        private readonly int _messageWait = 10;
        private readonly int _maxEmpty = 2;
        private readonly int _emptyWait = 5;
        private readonly string _topic = "deleteduserid";
        private readonly string _groupId = "G8";

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
                Guid userId = Guid.Empty;
                var message = _consumerHelper.ConsumeMessage(_topic, _groupId);
                if (message != null)
                    userId = new Guid(message);

                if (userId != Guid.Empty)
                {
                    var ticketsToDelete = _ticketDal.GetAllByUserId(userId);
                    foreach (var ticket in ticketsToDelete)
                        _ticketDal.Delete(ticket.Id);
                    messageCount++;
                    emptyCount = 0;
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromMinutes(_emptyWait));
                    emptyCount++;
                    continue;
                }
                if (messageCount == _maxMessage || emptyCount == _maxEmpty)
                {
                    _hostApplicationLifetime.StopApplication();
                }
                Thread.Sleep(TimeSpan.FromSeconds(_messageWait));
            }
        }
    }
}