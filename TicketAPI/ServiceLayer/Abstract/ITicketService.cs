using Entities.Concrete;

namespace ServiceLayer.Abstract
{
    public interface ITicketService
    {
        void Add(Ticket ticket, Guid userId);
        void Delete(Guid id);
        IList<Ticket> GetAll();
        void AddAnswer(Guid ticketId, Guid userId, Answer answer);
        Ticket GetById(Guid id);
        IList<Ticket> GetAllByUserId(Guid userId);
        void DeleteAllByUserId(Guid id);

    }
}
