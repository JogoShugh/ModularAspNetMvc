using System.ComponentModel.Composition;
using FieldReporting.Infrastructure.Ui.Mvc.Modularity;
using FieldReporting.Modules.UserProfile.Ui.Mvc.Controllers;

namespace FieldReporting.Modules.UserProfile.Ui.Mvc
{
    [Export(typeof(IModule))]
    public class Module : IModule
    {
        public void Initialize(ModuleLoader moduleLoader)
        {
            moduleLoader.MapCodeRoutes("UserProfile", typeof(UserProfileController));
        }
    }
}