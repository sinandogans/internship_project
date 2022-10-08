using System.Net;

namespace ServiceLayer.Utilities.ExceptionHandling
{
    public class MyException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public int ErrorCode { get; set; }

        public MyException(string message, HttpStatusCode statusCode, int errorCode) : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
        //public MyException(string message, int errorCode) : base(message)
        //{
        //    StatusCode = HttpStatusCode.InternalServerError;
        //    ErrorCode = errorCode;
        //}
        //public MyException(string? message) : base(message)
        //{
        //}

        //public MyException(string? message, Exception? innerException) : base(message, innerException)
        //{
        //}
    }
}
