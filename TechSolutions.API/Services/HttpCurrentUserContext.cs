using System.Linq;
using Microsoft.AspNetCore.Http;
using TechSolutions.Core.Security;

namespace TechSolutions.API.Services
{
    /// <summary>
    /// Implementación de ICurrentUserContext que obtiene la info del usuario
    /// desde el HttpContext. Si no hay autenticación, devuelve un usuario con rol Manager.
    /// </summary>
    public class HttpCurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpCurrentUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserContext GetCurrentUser()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            // Sin contexto HTTP o usuario no autenticado → rol por defecto Manager
            if (httpContext?.User?.Identity == null || !httpContext.User.Identity.IsAuthenticated)
            {
                return new UserContext("anonymous", UserRole.Manager);
            }

            var userName = httpContext.User.Identity.Name ?? "anonymous";

            // Intentar leer el claim "role" (opcional)
            var roleClaim = httpContext.User.Claims
                .FirstOrDefault(c => c.Type == "role")
                ?.Value;

            // Sólo manejamos Manager, cualquier otro valor cae a Manager.
            var role = roleClaim switch
            {
                "Manager" => UserRole.Manager,
                _         => UserRole.Manager
            };

            return new UserContext(userName, role);
        }
    }
}
