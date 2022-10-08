using DataAccess.MongoDb.Abstract;
using DataAccess.MongoDb.Models;
using Entities.Concrete;
using Microsoft.Extensions.Options;

namespace DataAccess.MongoDb.Concrete
{
    public class TicketDal : MongoDbBaseRepository<Ticket>, ITicketDal
    {
        public TicketDal(IOptions<StajProjectDbSettings> dbSettings) : base(dbSettings, dbSettings.Value.TicketsCollectionName)
        {
        }
    }
}
