using System.Security.Claims;
using ServiceLayer.Utilities.JWT;
using Microsoft.AspNetCore.Http;
using ServiceLayer.Utilities.ExceptionHandling;
using System.Net;

namespace ServiceLayer.Utilities.Authorization
{
    public class AuthorizationManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtValidator _jwtValidator;
        public AuthorizationManager(JwtValidator jwtValidator, IHttpContextAccessor httpContextAccessor)
        {
            _jwtValidator = jwtValidator;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Authorize(params string[] roles)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
                throw new MyException("Token is not found.", HttpStatusCode.InternalServerError, ErrorCodes.TokenNotFound);

            var jwtToken = _jwtValidator.ValidateToken(token);

            var userTypes = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            foreach (var userType in userTypes)
            {
                if (roles.Contains(userType.Value))
                {
                    return;
                }
            }
            throw new MyException("Authorization denied.", HttpStatusCode.InternalServerError, ErrorCodes.AuthorizationDenied);
        }
    }
}
