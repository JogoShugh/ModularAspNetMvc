using System;
using System.Web;
using System.Web.Security;

namespace FieldReporting.Infrastructure.Security
{
    public class FieldReportingFormsAuthentication
    {
        public void SetCookieFromPrincipal(IFieldReportingPrincipal principal)
        {
            if (HttpContext.Current != null)
            {
                var cookie = CreateCookie(FormsAuthentication.FormsCookieName, principal.Identity.Name);

                HttpContext.Current.Response.AppendCookie(cookie);            
            }
        }

        public void ResetPrincipalFromTicketIfExists()
        {
            if (FieldReportingPrincipal.CurrentPrincipal != null)
            {
                return;
            }

            var ticket = GetCurrentAuthenticationTicket();

            if (ticket != null)
            {
                var userName = ticket.Name;

                var principal = new FieldReportingPrincipalFactory().CreatePrincipal(userName);

                FieldReportingPrincipal.CurrentPrincipal = principal;
            }
        }

        private static FormsAuthenticationTicket GetCurrentAuthenticationTicket()
        {
            if (HttpContext.Current != null)
            {
                var authenticationCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

                return authenticationCookie != null ? FormsAuthentication.Decrypt(authenticationCookie.Value) : null;
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
            var encryptedTicket = FormsAuthentication.Encrypt(formsAuthenticationTicket);

            var authenticationCookie = new HttpCookie(cookieName, encryptedTicket)
                                           {
                                               Secure = false,
                                               HttpOnly = true
                                           };

            return authenticationCookie;
        }
    }
}