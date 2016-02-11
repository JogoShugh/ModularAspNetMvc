using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModulesDeploy
{
    public class Program
    {
        static void Main(string[] args)
        {
            var binPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Core.Ui.Mvc", "bin");
            var modulesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules.Deploy");
            var dirs = System.IO.Directory.GetDirectories(modulesPath);
            foreach (var dir in dirs)
            {
                var files = System.IO.Directory.GetFiles(dir);
                foreach (var file in files)
                {
                    var fileInfo = new System.IO.FileInfo(file);
                    System.Console.WriteLine("Copying " + file);
                    fileInfo.CopyTo(
                        System.IO.Path.Combine(binPath, fileInfo.Name), true);
                }
            }
        }
    }
}
