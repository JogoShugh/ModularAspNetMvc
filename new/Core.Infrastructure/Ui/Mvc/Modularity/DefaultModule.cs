using System.ComponentModel.Composition;

namespace Core.Infrastructure.Ui.Mvc.Modularity
{
    [Export(typeof(IModule))]
    public abstract class DefaultModule<TBaseControllerType> : IModule
    {
        protected abstract string GetBaseControllerRoute();

        public void Initialize(ModuleLoader moduleLoader)
        {
            var baseControllerRoute = GetBaseControllerRoute();
            moduleLoader.MapCodeRoutes(baseControllerRoute, typeof(TBaseControllerType));
        }
    }
}
