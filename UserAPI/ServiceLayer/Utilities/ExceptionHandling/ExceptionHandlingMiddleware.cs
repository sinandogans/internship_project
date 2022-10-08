using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace ServiceLayer.Utilities.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        RequestDelegate _next;
        string _message;
        int _statusCode;
        int _code = 0;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                _message = ex.Errors.First().ErrorMessage;
                _statusCode = (int)HttpStatusCode.InternalServerError;
                _code = 1;
                await HandleExceptionAsync(httpContext, _message, _statusCode, _code);
            }
            catch (MyException ex)
            {
                _message = ex.Message;
                _statusCode = (int)ex.StatusCode;
                _code = ex.ErrorCode;
                await HandleExceptionAsync(httpContext, _message, _statusCode, _code);
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                _statusCode = (int)HttpStatusCode.InternalServerError;
                _code = 0;
                await HandleExceptionAsync(httpContext, _message, _statusCode, _code);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, string message, int statusCode, int code)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            var jsonString = JsonSerializer.Serialize(new ExceptionModel()
            {
                Message = message,
                StatusCode = statusCode,
                ErrorCode = code
            });
            return httpContext.Response.WriteAsync(jsonString);
        }
    }
}
