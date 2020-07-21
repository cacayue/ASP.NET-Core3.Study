using System;
using Microsoft.Extensions.DependencyInjection;
using DI;

namespace EasyDI
{
    public class CatServiceProviderFactory: IServiceProviderFactory<CatBuilder>
    {
        public CatBuilder CreateBuilder(IServiceCollection services)
        {
            var cat = new Cat();
            foreach (var service in services)
            {
                if (service.ImplementationFactory != null)
                {
                    cat.Register(service.ServiceType, provider =>
                            service.ImplementationFactory(provider),
                       service.Lifetime.AsCatLigLifeTime());
                }
                else if(service.ImplementationInstance != null)
                {
                    cat.Register(service.ServiceType, service.ImplementationInstance);
                }
                else
                {
                    cat.Register(service.ServiceType, service.ImplementationType,
                        service.Lifetime.AsCatLigLifeTime());
                }
            }
            return new CatBuilder(cat);
        }

        public IServiceProvider CreateServiceProvider(CatBuilder containerBuilder)
            => containerBuilder.BuilderServiceProvider();
    }
}