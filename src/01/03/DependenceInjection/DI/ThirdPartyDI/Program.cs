using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EasyDI;
using EasyDI.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace ThirdPartyDI
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddTransient<IFoo, Foo>()
                .AddScoped<IBar>(_ => new Bar())
                .AddSingleton<IBaz>(new Baz());
            var factory = new CatServiceProviderFactory();
            IEnumerable<Assembly> allAssemblies = Assembly.GetEntryAssembly()?.GetReferencedAssemblies().Select(Assembly.Load);
            Assembly assembly = null;
            if (allAssemblies!= null && allAssemblies.Any())
            {
                assembly = allAssemblies
                    .FirstOrDefault(m => m.FullName.Contains("EasyDI"));
            }

            var builder = factory.CreateBuilder(services)
                .Register(assembly);
            
            var container = factory.CreateServiceProvider(builder);

            GetServices();
            GetServices();

            Console.WriteLine("\nRoot container is disposed");
            (container as IDisposable)?.Dispose();

            void GetServices()
            {
                using (var scope = container.CreateScope())
                {
                    Console.WriteLine("\nService scope is created");
                    var child = scope.ServiceProvider;

                    child.GetService<IFoo>();
                    child.GetService<IBar>();
                    child.GetService<IBaz>();
                    child.GetService<IQux>();
                    Console.WriteLine();
                    child.GetService<IFoo>();
                    child.GetService<IBar>();
                    child.GetService<IBaz>();
                    child.GetService<IQux>();
                    Console.WriteLine("\nService scope is disposed");
                }
            }
        }
    }
}
