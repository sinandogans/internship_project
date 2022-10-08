using DataAccess.Entities;

namespace DataAccess.MongoDb.Abstract
{
    public interface ITicketDal : IEntityRepository<Ticket>
    {
        IList<Ticket> GetAllByUserId(Guid userId);
    }
}
