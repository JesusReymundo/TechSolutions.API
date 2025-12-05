namespace TechSolutions.Core.Security
{
    public interface ICurrentUserContext
    {
        UserContext GetCurrentUser();
    }
}
