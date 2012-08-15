using System.Security.Principal;

namespace FieldReporting.Infrastructure.Security
{
    public class FieldReportingPrincipalFactory
    {
        public IFieldReportingPrincipal CreatePrincipal(string userName)
        {
            var identity = new GenericIdentity(userName);

            var principal = new FieldReportingPrincipal(identity, new string[] {});

            return principal;
        }
    }
}

