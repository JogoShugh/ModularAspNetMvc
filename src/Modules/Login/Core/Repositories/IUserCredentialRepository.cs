namespace Core.Modules.Authentication.Core.Repositories
{
    public interface IUserCredentialRepository
    {
        bool Authenticate(string userName, string password);
    }
}