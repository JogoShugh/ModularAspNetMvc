using System.ComponentModel.Composition;
using Core.Infrastructure.Ui.Mvc.Modularity;
using Core.Modules.StandardReport.Ui.Mvc.Controllers;

namespace Core.Modules.StandardReport.Ui.Mvc
{
    [Export(typeof(IModule))]
    public class Module : IModule
    {
        public void Initialize(ModuleLoader moduleLoader)
        {
            moduleLoader.MapCodeRoutes("StandardReport", typeof(StandardReportController));
        }
    }
}