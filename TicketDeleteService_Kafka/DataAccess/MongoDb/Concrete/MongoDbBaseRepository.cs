using DataAccess.MongoDb.Abstract;
using DataAccess.MongoDb.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace DataAccess.MongoDb.Concrete
{
    public class MongoDbBaseRepository<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, new()
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly string _collectionName;

        public MongoDbBaseRepository(IOptions<StajProjectDbSettings> dbSettings, string collectionName)
        {
            _collectionName = collectionName;
            MongoClient client = new MongoClient(dbSettings.Value.ConnectionStrings);
            IMongoDatabase database = client.GetDatabase(dbSettings.Value.DatabaseName);
            _collection = database.GetCollection<TEntity>(_collectionName);
        }

        public void Add(TEntity entity)
        {
            _collection.InsertOne(entity);
        }

        public void Delete(Guid id)
        {
            var deleteFilter = Builders<TEntity>.Filter.Eq("Id", id);
            _collection.DeleteOne(deleteFilter);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return _collection.Find(filter).SingleOrDefault();
        }

        public IList<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null!)
        {
            IList<TEntity> data;
            data =
                filter != null
                ? _collection.Find(filter).ToList()
                : _collection.Find(d => true).ToList();
            return data;
        }

        public void Update(Guid id, TEntity entity)
        {
            var updateFilter = Builders<TEntity>.Filter.Eq("Id", id);

            _collection.ReplaceOne(updateFilter, entity);
        }
    }
}
