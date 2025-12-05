namespace TechSolutions.Core.Security
{
    public class UserContext
    {
        public string UserName { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Viewer;
    }
}
