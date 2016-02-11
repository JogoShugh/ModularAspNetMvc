using System.ComponentModel.Composition;
using FieldReporting.Infrastructure.Ui.Mvc.Modularity;
using FieldReporting.Modules.StandardReport.Ui.Mvc.Controllers;

namespace FieldReporting.Modules.StandardReport.Ui.Mvc
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