using Entities.Concrete;
using ServiceLayer.Utilities.ExceptionHandling;
using System.Net;

namespace ServiceLayer.Utilities.Authentication
{
    public class AuthenticationManager
    {
        public void AuthenticateUser(string password, User user)
        {
            if (user.Password != password)
                throw new MyException("Password is not correct.", HttpStatusCode.BadRequest, ErrorCodes.IncorrectPassword);
        }
    }
}
