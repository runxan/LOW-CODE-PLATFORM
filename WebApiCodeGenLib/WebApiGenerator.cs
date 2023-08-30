using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebApiCodeGenLib
{
    public class WebApiGenerator
    {
        string _controllerName;
        string _controllerCode;

        public WebApiGenerator(string controllerName, string controllerCode)
        {
            _controllerName = controllerName.Replace(" ", string.Empty);
            _controllerCode = controllerCode;
        }

        public Assembly CreateDll()
        {
            IDictionary<string, string> compParams =
                 new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } };
            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp", compParams);
            string outputDll = _controllerName + ".dll";

            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = outputDll;
            parameters.ReferencedAssemblies.Add(@"System.dll");
            parameters.ReferencedAssemblies.Add(@"System.Net.Http.dll");
            parameters.ReferencedAssemblies.Add(@"System.Net.Http.WebRequest.dll");
            parameters.ReferencedAssemblies.Add(@"System.Net.Http.Formatting.dll");
            parameters.ReferencedAssemblies.Add(@"System.Web.Http.dll");
            parameters.ReferencedAssemblies.Add(@"System.Linq.dll");
            parameters.ReferencedAssemblies.Add(@"WebApiCodeGenLib.dll");
            parameters.ReferencedAssemblies.Add(@"Microsoft.CSharp.dll");
            parameters.ReferencedAssemblies.Add(@"Newtonsoft.Json.dll");
            parameters.ReferencedAssemblies.Add(@"System.Text.Json.dll");
            string code = new StringBuilder()
                .AppendLine(_controllerCode)
                .ToString();
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, code);
            if (results.Errors.Count > 0)
            {
                Console.WriteLine("Build Failed of "+_controllerName);
                foreach (CompilerError CompErr in results.Errors)
                {
                    Console.WriteLine(
                                "Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine);
                }
            }
            else
            {
                Console.WriteLine("Build Succeeded of "+_controllerName);
                return Assembly.LoadFrom(outputDll);
            }
            return null;
        }
    }
}
