using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Core.Infrastructure.Composition
{
    public class SimpleComposer
    {
        public void ComposeParts(object target)
        {
            var directoryCatalog =
                new DirectoryCatalog(
                    @"c:\users\jgough\documents\visual studio 2010\Projects\Core\Core.Ui.Mvc\bin");
            var container = new CompositionContainer(directoryCatalog);
            container.ComposeParts(target);
        }
    }
}