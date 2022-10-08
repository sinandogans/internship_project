using Entities.Concrete;

namespace ServiceLayer.Abstract
{
    public interface IUserService
    {
        void Add(User user);
        IList<User> GetAll();
        void Delete(Guid id);
        void Authorize(Guid id);
        bool IsUserAlreadyRegistered(User user);
        User GetById(Guid id);
        User GetByEmail(string email);
        User GetByUsername(string username);
        bool CheckIfIsAdmin(Guid id);
        string Login(string email, string password);
    }
}
