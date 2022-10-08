using System.Net;

namespace ServiceLayer.Utilities.ExceptionHandling
{
    public class BadRequest : MyException
    {
        public BadRequest(string message = "Value is not valid.", int errorCode = 400) : base(message, HttpStatusCode.BadRequest, errorCode)
        {

        }
    }

    public class EmailNotValid : BadRequest
    {
        public EmailNotValid(string email) : base($"Email is not valid with value {email}", 401)
        {

        }
    }
}
