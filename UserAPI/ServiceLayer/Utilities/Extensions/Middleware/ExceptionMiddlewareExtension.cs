using Microsoft.AspNetCore.Builder;
using ServiceLayer.Utilities.ExceptionHandling;

namespace ServiceLayer.Utilities.Extensions.Middleware
{
    public static class ExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
