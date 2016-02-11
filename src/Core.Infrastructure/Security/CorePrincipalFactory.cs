using System.Security.Principal;

namespace Core.Infrastructure.Security
{
    public class CorePrincipalFactory
    {
        public ICorePrincipal CreatePrincipal(string userName)
        {
            var identity = new GenericIdentity(userName);

            var principal = new CorePrincipal(identity, new string[] {});

            return principal;
        }
    }
}

