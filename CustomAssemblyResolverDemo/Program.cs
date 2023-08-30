using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using WebApiCodeGenLib;
using WebApiCodeGenLib.Helpers;

namespace CustomAssemblyResolver
{
    class Program
    {
        static readonly Uri _baseAddress = new Uri("http://localhost:60064");

        static void Main(string[] args)
        {

            HttpSelfHostServer server = null;
            try
            {
                // Set up server configuration
                HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseAddress);
                config.HostNameComparisonMode = HostNameComparisonMode.Exact;
                config.ClientCredentialType = HttpClientCredentialType.None;
                var cors = new EnableCorsAttribute("*", "*", "*");
                config.EnableCors(cors);
                config.Routes.MapHttpRoute(
                 name: "DefaultApi",
                 routeTemplate: "api/{controller}/{action}/{id}",
                 defaults: new { id = RouteParameter.Optional }
                );
                var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
                config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

                // Set our own assembly resolver where we add the assemblies we need           
                DynamicAssemblyResolver assemblyResolver = new DynamicAssemblyResolver();
                config.Services.Replace(typeof(IAssembliesResolver), assemblyResolver);
                // Create server
                server = new HttpSelfHostServer(config);
                // Start listening
                server.OpenAsync().Wait();
                Console.WriteLine("Listening on " + _baseAddress);
                while (true)
                {
                    // Run HttpClient issuing requests
                   // RunDynamicClientAsync();
                    Console.WriteLine("Press Ctrl+C to exit...");
                    assemblyResolver.GetAssemblies(); 
                    Console.ReadLine();
                }
              
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not start server: {0}", e.GetBaseException().Message);
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();
            }
            finally
            {
                if (server != null)
                {
                    // Stop listening
                    server.CloseAsync().Wait();
                }
            }
        }

        static async void RunDynamicClientAsync()
        {
            // Create an HttpClient instance
            HttpClient client = new HttpClient();

            // Send GET request to server for the hello controller which lives in the controller library
            Uri address = new Uri(_baseAddress, "/api/SayHello/Get");
            HttpResponseMessage response = await client.GetAsync(address);

            // Ensure we get a successful response.
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            // Ensure we get a successful response.
            response.EnsureSuccessStatusCode();

            content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("API working, Test api is returning : {0}", content);
        }
    }
}
