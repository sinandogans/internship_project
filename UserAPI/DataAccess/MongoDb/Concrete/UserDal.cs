using DataAccess.MongoDb.Abstract;
using DataAccess.MongoDb.Models;
using Entities.Concrete;
using Microsoft.Extensions.Options;

namespace DataAccess.MongoDb.Concrete
{
    public class UserDal : MongoDbBaseRepository<User>, IUserDal
    {
        public UserDal(IOptions<StajProjectDbSettings> dbSettings) : base(dbSettings, dbSettings.Value.UserCollectionName)
        {
        }
    }
}
