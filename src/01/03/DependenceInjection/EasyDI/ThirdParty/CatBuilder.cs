using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using DI;

namespace EasyDI
{
    public class CatBuilder
    {
        private class ServiceScope : IServiceScope
        {
            public ServiceScope(IServiceProvider serviceProvider)
            {
                ServiceProvider = serviceProvider;
            }

            public IServiceProvider ServiceProvider { get; }

            public void Dispose()
            {
                (ServiceProvider as IDisposable)?.Dispose();
            }
        }

        private class ServiceScopeFactory : IServiceScopeFactory
        {
            private readonly Cat _cat;

            public ServiceScopeFactory(Cat cat)
            {
                _cat = cat;
            }

            public IServiceScope CreateScope()
            {
                return new ServiceScope(_cat);
            }
        }

        private readonly Cat _cat;

        public CatBuilder(Cat cat)
        {
            _cat = cat;
            _cat.Register<IServiceScopeFactory>(c => new ServiceScopeFactory(c.CreateChild()), LifeTime.Transient);
        }

        public IServiceProvider BuilderServiceProvider() => _cat;

        public CatBuilder Register(Assembly assembly)
        {
            _cat.Register(assembly);
            return this;
        }
    }
}