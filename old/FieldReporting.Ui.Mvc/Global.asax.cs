using System;
using System.Web.Mvc;
using System.Web.Routing;
using FieldReporting.Infrastructure.Boot;
using FieldReporting.Infrastructure.Security;
using MvcCodeRouting;

namespace FieldReporting.Ui.Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // NOTE: Take this out because MvcCodeRouting is doing our work now...
            //routes.MapRoute(
            //    "Default", // Route name
            //    "{controller}/{action}/{id}", // URL with parameters
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            //);

            ViewEngines.Engines.EnableCodeRouting();

            new BootStrapper().Boot();
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var factory = new FieldReportingFormsAuthentication();
            factory.ResetPrincipalFromTicketIfExists();
        }

        protected void Application_Start()
        {
            AuthenticateRequest += Application_AuthenticateRequest;

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}