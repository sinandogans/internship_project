using Microsoft.Extensions.Hosting;
using ServiceLayer.Abstract;
using ServiceLayer.Utilities.MessageBroker.RabbitMQ;

namespace ServiceLayer.Utilities.Workers
{
    public class DeleteAllTicketsByUserIdWorker : BackgroundService
    {
        private readonly MqConsumerHelper _consumerHelper;
        private readonly ITicketService _ticketService;
        private CancellationTokenSource _cancellationTokenSource;
        CancellationToken _cancellationToken;
        private readonly int _maxMessage = 10;
        private readonly int _messageWait = 2;
        private readonly int _maxEmpty = 2;
        private readonly int _emptyWait = 5;
        private readonly string _queueName = "q2";
        public DeleteAllTicketsByUserIdWorker(MqConsumerHelper consumerHelper, ITicketService ticketService)
        {
            _consumerHelper = consumerHelper;
            _ticketService = ticketService;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }
        protected override Task ExecuteAsync(CancellationToken token)
        {
            int messageCount = 0;
            int emptyCount = 0;
            while (!_cancellationToken.IsCancellationRequested)
            {
                var userId = _consumerHelper.ConsumeMessage(_queueName);
                if (userId != Guid.Empty)
                {
                    var ticketsToDelete = _ticketService.GetAllByUserId(userId);
                    foreach (var ticket in ticketsToDelete)
                        _ticketService.Delete(ticket.Id);
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
                    _cancellationTokenSource.Cancel();
                }
                Thread.Sleep(TimeSpan.FromMinutes(_messageWait));
            }
            return Task.CompletedTask;
        }
    }
}
