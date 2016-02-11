using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace FieldReporting.Infrastructure.Composition
{
    public class PartsAssembler
    {
        public void ComposeParts(object target)
        {
            var directoryCatalog =
                new DirectoryCatalog(
//                    @"C:\Users\JGough\Documents\Visual Studio 2010\Projects\FieldReporting\ModulesDeploymentFolder\");
                    @"C:\users\jgough\documents\visual studio 2010\Projects\FieldReporting\FieldReporting.Ui.Mvc\bin");
            var container = new CompositionContainer(directoryCatalog);
            container.ComposeParts(target);
        }
    }
}