using System.Security.Principal;
using System.Web;

namespace FieldReporting.Infrastructure.Security
{
    public class FieldReportingPrincipal : GenericPrincipal, IFieldReportingPrincipal
    {
        public FieldReportingPrincipal(IIdentity identity, string[] roles) : base(identity, roles)
        {
        }

        private static IFieldReportingPrincipal _currentPrincipal;
        private static readonly object Lock = new object();

        public static IFieldReportingPrincipal CurrentPrincipal
        {
            get
            {
                lock (Lock)
                {
                    if (_currentPrincipal == null)
                    {
                        if (HttpContext.Current != null)
                        {
                            return HttpContext.Current.User as IFieldReportingPrincipal;
                        }

                        return System.Threading.Thread.CurrentPrincipal as IFieldReportingPrincipal;
                    }
                    return _currentPrincipal;
                }
            }
            set
            {
                lock (Lock)
                {
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = value;
                    }
                    else
                    {
                        System.Threading.Thread.CurrentPrincipal = value;
                    }
                }
            }
        }

        public static void SetCurrentPrincipalManually(IFieldReportingPrincipal principal)
        {
            lock (Lock)
            {
                _currentPrincipal = principal;
            }
        }
    }
}
