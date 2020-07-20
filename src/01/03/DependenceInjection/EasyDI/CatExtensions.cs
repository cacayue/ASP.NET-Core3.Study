using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using EasyDI;

namespace DI
{
    public static class CatExtensions
    {
        public static Cat Register(this Cat cat,Type from,Type @to,LifeTime lifeTime)
        {
            Func<Cat, Type[], object> factory = (_, arguments) => Create(_, @to, arguments);
            cat.Register(new ServiceRegistry(from, lifeTime, factory ));
            return cat;
        }

        public static Cat Register<TFrom, TTo>(this Cat cat, LifeTime lifeTime) where TTo:TFrom
        {
            cat.Register(typeof(TFrom), typeof(TTo), lifeTime);
            return cat;
        }

        public static Cat Register(this Cat cat, Type serviceType, object instance)
        {
            Func<Cat, Type[], object> factory = (_, arguments) => instance;
            cat.Register(new ServiceRegistry(serviceType, LifeTime.Root, factory));
            return cat;
        }

        public static Cat Register<TService>(this Cat cat, TService instance)
        {
            Func<Cat, Type[], object> factory = (_, arguments) => instance;
            cat.Register(new ServiceRegistry(typeof(TService), LifeTime.Root, factory));
            return cat;
        }

        public static Cat Register(this Cat cat, Type serviceType,
            Func<Cat, object> factory, LifeTime lifeTime)
        {
            cat.Register(new ServiceRegistry(serviceType, lifeTime, (_,arguments)=>factory(_)));
            return cat;
        }

        public static Cat Register<TService>(this Cat cat,
            Func<Cat, TService> factory, LifeTime lifeTime)
        {
            cat.Register(new ServiceRegistry(typeof(TService), lifeTime, (_, arguments) => factory(_)));
            return cat;
        }

        public static Cat Register(this Cat cat, Assembly assembly)
        {
            var typedAttributes = from type in assembly.GetExportedTypes()
                let attribute = type.GetCustomAttribute<MapToAttribute>()
                where attribute != null
                select new {ServiceType = type, Attribute = attribute};
            foreach (var typedAttribute in typedAttributes)
            {
                cat.Register(typedAttribute.Attribute.ServiceType,
                    typedAttribute.ServiceType, typedAttribute.Attribute.LifeTime);
            }

            return cat;
        }

        public static T GetService<T>(this Cat cat) => (T) cat.GetService(typeof(T));
        public static IEnumerable<T> GetServices<T>(this Cat cat) => cat.GetService<IEnumerable<T>>();
        public static Cat CreateChild(this Cat cat)=>new Cat(cat);

        private static object Create(Cat cat, Type type, Type[] genericArguments)
        {
            if (genericArguments.Length > 0)
            {
                type = type.MakeGenericType(genericArguments);
            }

            var constructors = type.GetConstructors();
            if (constructors.Length == 0)
            {
                throw new InvalidOperationException($"Cannot create this instance of" +
                                                    $"{type} which does not have a public constructor");
            }

            var constructor = constructors.FirstOrDefault(it =>
                it.GetCustomAttributes(false).OfType<InjectionAttribute>().Any());
            constructor = constructors.First() ?? constructors.First();
            var parameters = constructor.GetParameters();
            if (parameters.Length == 0)
            {
                return Activator.CreateInstance(type);
            }

            var arguments = new object[parameters.Length];
            for (int index = 0; index < arguments.Length; index++)
            {
                arguments[index] = cat.GetService(parameters[index].ParameterType);
            }

            return constructor.Invoke(arguments);
        }
    }
}