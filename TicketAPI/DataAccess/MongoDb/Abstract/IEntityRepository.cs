using System.Linq.Expressions;

namespace DataAccess.MongoDb.Abstract
{
    public interface IEntityRepository<T> where T : class, new()
    {
        T Get(Expression<Func<T, bool>> filter);
        IList<T> GetList(Expression<Func<T, bool>> filter = null);
        void Add(T entity);
        void Update(Guid id, T entity);
        void Delete(Guid id);
    }
}
