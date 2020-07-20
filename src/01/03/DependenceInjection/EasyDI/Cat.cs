using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EasyDI
{
    public class Cat : IServiceProvider,IDisposable
    {
        internal readonly Cat _root;
        internal readonly ConcurrentDictionary<Type, ServiceRegistry> _registries;
        private readonly ConcurrentDictionary<Key, object> _services;
        private ConcurrentBag<IDisposable> _disposables;
        private volatile bool _disposed;

        public Cat()
        {
            _registries = new ConcurrentDictionary<Type, ServiceRegistry>();
            _root = this;
            _services = new ConcurrentDictionary<Key, object>();
            _disposables = new ConcurrentBag<IDisposable>();
        }

        internal Cat(Cat parent)
        {
            _root = parent._root;
            _registries = _root._registries;
            _services = new ConcurrentDictionary<Key, object>();
            _disposables = new ConcurrentBag<IDisposable>();
        }

        private void EnsureNotDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Cat));
            }
        }

        public Cat Register(ServiceRegistry registry)
        {
            EnsureNotDisposed();
            if (_registries.TryGetValue(registry.ServiceType, out var existing))
            {
                _registries[registry.ServiceType] = registry;
                //构建链表,将已存在的放入末尾
                registry.Next = existing;
            }
            else
            {
                _registries[registry.ServiceType] = registry;
            }

            return this;
        }

        private object GetServiceCore(ServiceRegistry registry,
            Type[] genericArguments)
        {
            var key = new Key(registry,genericArguments);
            var serviceType = registry.ServiceType;
            switch (registry.LifeTime)
            {
                case LifeTime.Root:
                    return GetOrCreate(_root._services, _root._disposables);
                case LifeTime.Self:
                    return GetOrCreate(_services, _disposables);
                default:
                {
                    var service = registry.Factory(this, genericArguments);
                    if (service is IDisposable disposable && disposable != this)
                    {
                        _disposables.Add(disposable);
                    }

                    return service;
                }
            }

            object GetOrCreate(ConcurrentDictionary<Key, object> services,
                ConcurrentBag<IDisposable> disposables)
            {
                if (services.TryGetValue(key, out var service))
                {
                    return service;
                }

                service = registry.Factory(this, genericArguments);
                services[key] = service;
                if (service is IDisposable disposable)
                {
                    disposables.Add(disposable);
                }

                return service;
            }
        }

        public object GetService(Type serviceType)
        {
            EnsureNotDisposed();
            if (serviceType == typeof(Cat) ||
                serviceType == typeof(IServiceProvider))
            {
                return this;
            }

            ServiceRegistry serviceRegistry;
            //IEnumerable
            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() ==
                typeof(IEnumerable<>))
            {
                var elementType = serviceType.GetGenericArguments()[0];
                if (!_registries.TryGetValue(elementType, out serviceRegistry))
                {
                    return Array.CreateInstance(elementType, 0);
                }

                var registries = serviceRegistry.AsEnumerable();
                var services = registries.Select(it => GetServiceCore(it, Type.EmptyTypes))
                    .ToArray();
                Array array = Array.CreateInstance(elementType,services.Length);
                services.CopyTo(array,0);
                return array;
            }
            //Generic
            if (serviceType.IsGenericType && !_registries.ContainsKey(serviceType))
            {
                var definition = serviceType.GetGenericTypeDefinition();
                return _registries.TryGetValue(definition, out serviceRegistry)
                    ? GetServiceCore(serviceRegistry, serviceType.GetGenericArguments())
                    : null;
            }
            //Normal
            return _registries.TryGetValue(serviceType, out serviceRegistry)
                ? GetServiceCore(serviceRegistry, new Type[0])
                : null;
        }

        public void Dispose()
        {
            _disposed = true;
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            var newBag = new ConcurrentBag<IDisposable>();
            Interlocked.Exchange(ref _disposables, newBag);
            _services.Clear();
        }
    }
}
