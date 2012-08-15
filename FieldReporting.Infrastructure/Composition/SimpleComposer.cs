using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace FieldReporting.Infrastructure.Composition
{
    public class SimpleComposer
    {
        public void ComposeParts(object target)
        {
            var directoryCatalog =
                new DirectoryCatalog(
                    @"c:\users\jgough\documents\visual studio 2010\Projects\FieldReporting\FieldReporting.Ui.Mvc\bin");
            var container = new CompositionContainer(directoryCatalog);
            container.ComposeParts(target);
        }
    }
}