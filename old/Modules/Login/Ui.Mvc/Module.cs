using System.ComponentModel.Composition;
using FieldReporting.Infrastructure.Ui.Mvc.Modularity;
using FieldReporting.Modules.Authentication.Ui.Mvc.Controllers;

namespace FieldReporting.Modules.Authentication.Ui.Mvc
{
    [Export(typeof(IModule))]
    public class Module : DefaultModule<AuthenticationController>
    {
        protected override string GetBaseControllerRoute()
        {
            return "Authentication";
        }
    }
}