using System;
using System.Web;
using System.Web.Security;

namespace Core.Infrastructure.Security
{
    public class CoreFormsAuthentication
    {
        public void SetCookieFromPrincipal(ICorePrincipal principal)
        {
            if (HttpContext.Current != null)
            {
                var cookie = CreateCookie(System.Web.Security.FormsAuthentication.FormsCookieName, principal.Identity.Name);

                HttpContext.Current.Response.AppendCookie(cookie);            
            }
        }

        public void ResetPrincipalFromTicketIfExists()
        {
            if (CorePrincipal.CurrentPrincipal != null)
            {
                return;
            }

            var ticket = GetCurrentAuthenticationTicket();

            if (ticket != null)
            {
                var userName = ticket.Name;

                var principal = new CorePrincipalFactory().CreatePrincipal(userName);

                CorePrincipal.CurrentPrincipal = principal;
            }
        }

        private static FormsAuthenticationTicket GetCurrentAuthenticationTicket()
        {
            if (HttpContext.Current != null)
            {
                var authenticationCookie = HttpContext.Current.Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];

                return authenticationCookie != null ? System.Web.Security.FormsAuthentication.Decrypt(authenticationCookie.Value) : null;
            }

            return null;
        }

        private static HttpCookie CreateCookie(string cookieName, string userName)
        {
            const int minutesToStayValid = 15;

            var creationTime = DateTime.Now;
            var expires = creationTime.AddMinutes(minutesToStayValid);

            var formsAuthenticationTicket = new FormsAuthenticationTicket(1, userName, creationTime, 
                                                                          expires, false, string.Join("|", userName));
            var encryptedTicket = System.Web.Security.FormsAuthentication.Encrypt(formsAuthenticationTicket);

            var authenticationCookie = new HttpCookie(cookieName, encryptedTicket)
                                           {
                                               Secure = false,
                                               HttpOnly = true
                                           };

            return authenticationCookie;
        }
    }
}