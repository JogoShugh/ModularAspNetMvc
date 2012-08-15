namespace FieldReporting.Modules.Authentication.Core.Repositories
{
    public interface IUserCredentialRepository
    {
        bool Authenticate(string userName, string password);
    }
}