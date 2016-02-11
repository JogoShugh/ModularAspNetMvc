using System.ComponentModel.Composition;
using Core.Infrastructure.Ui.Mvc.Modularity;
using Core.Modules.UserProfile.Ui.Mvc.Controllers;

namespace Core.Modules.UserProfile.Ui.Mvc
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