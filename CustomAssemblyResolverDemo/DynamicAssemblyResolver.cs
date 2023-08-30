using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;
using WebApiCodeGenLib;
using System.Configuration;

namespace CustomAssemblyResolver
{
    public class DynamicAssemblyResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            ICollection<Assembly> baseAssemblies = base.GetAssemblies();
            List<Assembly> assemblies = new List<Assembly>(baseAssemblies);

            // Add our controller library assembly
            try
            {
                List<string> fileList = new List<string>();
                string path = ConfigurationManager.AppSettings["ServerLogicPath"];
                if (Directory.Exists(path))
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        string ext = Path.GetExtension(fi.FullName);
                        fileList.Add(Path.GetFileNameWithoutExtension(fi.FullName));
                        var code = "";
                        using (StreamReader sr = System.IO.File.OpenText(path+"\\"+fi.Name))
                        {
                            string s = "";
                            while ((s = sr.ReadLine()) != null)
                            {
                                code += s;
                            }
                        }
                        WebApiGenerator gen = new WebApiGenerator(Path.GetFileNameWithoutExtension(fi.FullName),code);
                        Assembly onTheFly = gen.CreateDll();
                        if (onTheFly != null)
                        {
                            assemblies.Add(onTheFly);
                        }
                    }
                }
               
            }
            catch
            {
                // We ignore errors and just continue
            }

            return assemblies;
        }
    }
}
