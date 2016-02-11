using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Core.Infrastructure.Composition
{
    public class PartsAssembler
    {
        public void ComposeParts(object target)
        {
            var agCatalog = new AggregateCatalog();
            var path = System.Web.HttpRuntime.BinDirectory;
            var container = new CompositionContainer(new DirectoryCatalog(path));
            container.ComposeParts(target);
        }

        //public void ComposeParts(object target)
        //{
        //    var agCatalog = new AggregateCatalog();
        //    var path = System.IO.Path.Combine(System.Web.HttpRuntime.BinDirectory, "");
        //    //var dirs = System.IO.Directory.GetDirectories(path);
        //    //foreach (var dir in dirs)
        //    //{
        //    //    var directoryCatalog = new DirectoryCatalog(dir);
        //    //    agCatalog.Catalogs.Add(directoryCatalog);
        //    //}

        //    //var container = new CompositionContainer(agCatalog);
        //    var container = new CompositionContainer(new DirectoryCatalog(path));
        //    container.ComposeParts(target);
        //}
    }
}