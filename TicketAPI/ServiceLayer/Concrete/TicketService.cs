using DataAccess.MongoDb.Abstract;
using Entities.Concrete;
using ServiceLayer.Abstract;
using ServiceLayer.Utilities.ExceptionHandling;
using ServiceLayer.Utilities.MessageBroker.Kafka;
using ServiceLayer.Utilities.Validation;
using System.Net;

namespace ServiceLayer.Concrete
{
    public class TicketService : ITicketService
    {
        private readonly ITicketDal _ticketDal;
        private readonly TicketValidationManager _ticketValidationManager;
        private readonly AnswerValidationManager _answerValidationManager;
        private readonly KafkaPublisherHelper _kafkaPublisherHelper;
        private readonly KafkaConsumerHelper _kafkaConsumerHelper;
        public TicketService(ITicketDal ticketDal, TicketValidationManager ticketValidationManager, AnswerValidationManager answerValidationManager, KafkaPublisherHelper kafkaPublisherHelper, KafkaConsumerHelper kafkaConsumerHelper)
        {
            _ticketDal = ticketDal;
            _ticketValidationManager = ticketValidationManager;
            _answerValidationManager = answerValidationManager;
            _kafkaPublisherHelper = kafkaPublisherHelper;
            _kafkaConsumerHelper = kafkaConsumerHelper;
        }

        public void Add(Ticket ticket, Guid userId)
        {
            _ticketValidationManager.Validate(ticket);

            HttpClient client = new HttpClient();
            var isUserAdminTask = client.PostAsync($"https://localhost:7186/api/users/checkifisadmin?id={userId}", null);
            isUserAdminTask.Wait();

            if (isUserAdminTask.Result.IsSuccessStatusCode)
                throw new MyException("Admins cannot create ticket.", HttpStatusCode.BadRequest, ErrorCodes.AdminsCannotCreateTicket);

            ticket.Id = Guid.NewGuid();
            ticket.CreatedBy = userId;
            ticket.CreatedAt = DateTime.Now;
            ticket.Answers = new List<Answer>();
            ticket.Status = "Ticket created";
            _ticketDal.Add(ticket);
        }

        public void Delete(Guid id)
        {
            var ticket = this.GetById(id);
            _ticketDal.Delete(id);
        }

        public void DeleteAllByUserId(Guid userId)
        {
            var tickets = this.GetAllByUserId(userId);
            foreach (var ticket in tickets)
                _ticketDal.Delete(ticket.Id);
        }

        public IList<Ticket> GetAll()
        {
            //_kafkaPublisherHelper.PublishMessage<string>("mesaj4", "weblog");
            //_kafkaConsumerHelper.ConsumeMessage();
            return _ticketDal.GetList();
        }

        public Ticket GetById(Guid id)
        {
            var ticket = _ticketDal.Get(t => t.Id == id);
            if (ticket == null)
                throw new MyException("Ticket is not found", HttpStatusCode.NotFound, ErrorCodes.TicketNotFound);
            return ticket;
        }

        public IList<Ticket> GetAllByUserId(Guid userId)
        {
            return _ticketDal.GetList(t => t.CreatedBy == userId);
        }

        public void AddAnswer(Guid ticketId, Guid userId, Answer answer)
        {
            _answerValidationManager.Validate(answer);

            var ticket = this.GetById(ticketId);

            HttpClient client = new HttpClient();

            var isUserAdminTask = client.PostAsync($"https://localhost:7186/api/users/checkifisadmin?id={userId}", null);
            isUserAdminTask.Wait();
            bool isUserAdmin = isUserAdminTask.Result.IsSuccessStatusCode;

            if (ticket.CreatedBy != userId && !isUserAdmin)
                throw new MyException("You do not have permission to reply this ticket.", HttpStatusCode.BadRequest, ErrorCodes.NoPermissionToReply);

            if (ticket.Answers.Count == 0 && !isUserAdmin)
                throw new MyException("Wait for the admins reply.", HttpStatusCode.BadRequest, ErrorCodes.WaitForAdminsReply);

            if (ticket.Answers.Count != 0)
            {
                if (ticket.Answers.Last().CreatedBy == userId && isUserAdmin)
                    throw new MyException("Wait for the user's reply.", HttpStatusCode.BadRequest, ErrorCodes.WaitForUsersReply);
                else if (ticket.Answers.Last().CreatedBy == userId)
                    throw new MyException("Wait for the admins reply.", HttpStatusCode.BadRequest, ErrorCodes.WaitForAdminsReply);
            }

            answer.CreatedAt = DateTime.Now;
            answer.Id = Guid.NewGuid();
            answer.CreatedBy = userId;

            ticket.UpdatedAt = answer.CreatedAt;
            ticket.Answers.Add(answer);
            ticket.Status = "Replied";
            this.Update(ticket);
        }
        void Update(Ticket ticket)
        {
            _ticketDal.Update(ticket.Id, ticket);
        }
    }
}
