using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using TechSolutions.Core.Security;

namespace TechSolutions.API.Services
{
    // Implementa ICurrentUserContext leyendo datos del request
    // (querystring o headers)
    public class HttpCurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpCurrentUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserContext GetCurrentUser()
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("No HttpContext available.");

            var headers = httpContext.Request.Headers;
            var query = httpContext.Request.Query;

            // Permite probar fácil desde Swagger:
            // /api/Reports/monthly?year=2025&month=12&userName=Ana&role=Manager
            var userName =
                query["userName"].FirstOrDefault() ??
                headers["X-User-Name"].FirstOrDefault() ??
                "Invitado";

            var roleHeader =
                query["role"].FirstOrDefault() ??
                headers["X-User-Role"].FirstOrDefault() ??
                "Viewer";

            if (!Enum.TryParse<UserRole>(roleHeader, ignoreCase: true, out var role))
            {
                role = UserRole.Viewer;
            }

            return new UserContext
            {
                UserName = userName,
                Role = role
            };
        }
    }
}
