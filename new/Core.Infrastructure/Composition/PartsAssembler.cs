using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Core.Infrastructure.Composition
{
    public class PartsAssembler
    {
        public void ComposeParts(object target)
        {
            
            var directoryCatalog =
                new DirectoryCatalog(System.Web.HttpRuntime.BinDirectory);
//                    @"C:\Users\JGough\Documents\Visual Studio 2010\Projects\Core\ModulesDeploymentFolder\");
            var container = new CompositionContainer(directoryCatalog);
            container.ComposeParts(target);
        }
    }
}