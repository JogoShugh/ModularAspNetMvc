using System.ComponentModel.Composition;
using Core.Infrastructure.Ui.Mvc.Modularity;
using Core.Modules.Authentication.Ui.Mvc.Controllers;

namespace Core.Modules.Authentication.Ui.Mvc
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