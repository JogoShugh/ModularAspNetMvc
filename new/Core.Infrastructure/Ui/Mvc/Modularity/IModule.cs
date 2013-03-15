namespace Core.Infrastructure.Ui.Mvc.Modularity
{
    public interface IModule
    {
        void Initialize(ModuleLoader moduleLoader);
    }
}
