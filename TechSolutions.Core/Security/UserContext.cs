using System;

namespace TechSolutions.Core.Security
{
    /// <summary>
    /// Representa al usuario actual que consume el sistema.
    /// </summary>
    public class UserContext
    {
        // Nombre de usuario (no null)
        public string UserName { get; set; } = string.Empty;

        // Rol del usuario (Admin, Manager, etc.)
        public UserRole Role { get; set; }

        public UserContext()
        {
            // Rol por defecto: Manager (ajusta si quieres otro)
            Role = UserRole.Manager;
        }

        public UserContext(string userName, UserRole role)
        {
            UserName = userName;
            Role = role;
        }

        public bool IsInRole(UserRole role)
        {
            return Role == role;
        }
    }
}
