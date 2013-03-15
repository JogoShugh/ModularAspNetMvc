using System.Security.Principal;
using System.Web;

namespace Core.Infrastructure.Security
{
    public class CorePrincipal : GenericPrincipal, ICorePrincipal
    {
        public CorePrincipal(IIdentity identity, string[] roles) : base(identity, roles)
        {
        }

        private static ICorePrincipal _currentPrincipal;
        private static readonly object Lock = new object();

        public static ICorePrincipal CurrentPrincipal
        {
            get
            {
                lock (Lock)
                {
                    if (_currentPrincipal == null)
                    {
                        if (HttpContext.Current != null)
                        {
                            return HttpContext.Current.User as ICorePrincipal;
                        }

                        return System.Threading.Thread.CurrentPrincipal as ICorePrincipal;
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

        public static void SetCurrentPrincipalManually(ICorePrincipal principal)
        {
            lock (Lock)
            {
                _currentPrincipal = principal;
            }
        }
    }
}
