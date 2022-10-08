using DataAccess.Entities;
using DataAccess.MongoDb.Abstract;
using DataAccess.MongoDb.Models;
using Microsoft.Extensions.Options;

namespace DataAccess.MongoDb.Concrete
{
    public class TicketDal : MongoDbBaseRepository<Ticket>, ITicketDal
    {
        public TicketDal(IOptions<StajProjectDbSettings> dbSettings) : base(dbSettings, dbSettings.Value.TicketsCollectionName)
        {
        }

        public IList<Ticket> GetAllByUserId(Guid userId)
        {
            return base.GetList(t => t.CreatedBy == userId);
        }
    }
}
