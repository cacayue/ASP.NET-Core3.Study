using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DI;
using EasyDI.Interface;

namespace EasyDI
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-------test1---------");
            Test1();
            Console.WriteLine("-------test2---------");
            Test2();
            Console.WriteLine("-------test3---------");
            Test3();
            Console.WriteLine("-------test4---------");
            Test4();
            Console.ReadKey();
        }

        private static void Test1()
        {
            var root = new Cat()
                .Register<IFoo, Foo>(LifeTime.Transient)
                .Register<IBar>(_ => new Bar(), LifeTime.Self)
                .Register<IBaz, Baz>(LifeTime.Root)
                .Register(Assembly.GetEntryAssembly());
            var cat1 = root.CreateChild();
            var cat2 = root.CreateChild();
            Console.WriteLine("注册后的字典内容");
            foreach (var keyValuePair in root._registries)
            {
                Console.WriteLine(keyValuePair.Key + "" + keyValuePair.Value);
            }
            Console.WriteLine();
            void GetServices<TService>(Cat cat)
            {
                cat.GetService<TService>();
                cat.GetService<TService>();
            }

            GetServices<IFoo>(cat1);
            GetServices<IBar>(cat1);
            GetServices<IBaz>(cat1);
            GetServices<IQux>(cat1);
            Console.WriteLine();
            GetServices<IFoo>(cat2);
            GetServices<IBar>(cat2);
            GetServices<IBaz>(cat2);
            GetServices<IQux>(cat2);
        }

        private static void Test2()
        {
            var cat = new Cat()
                .Register<IFoo,Foo>(LifeTime.Transient)
                .Register<IBar,Bar>(LifeTime.Transient)
                .Register(typeof(IFoobar<,>),typeof(Foobar<,>),LifeTime.Transient);
            var foobar = (Foobar<IFoo, IBar>) cat.GetService<IFoobar<IFoo, IBar>>();
            Console.WriteLine("注册后的字典内容");
            foreach (var keyValuePair in cat._registries)
            {
                Console.WriteLine(keyValuePair.Key + "" + keyValuePair.Value);
            }

            Debug.Assert(foobar.Foo is Foo);
            Console.WriteLine(foobar.Foo is Foo);
            Debug.Assert(foobar.Bar is Bar);
            Console.WriteLine(foobar.Bar is Bar);
        }

        private static void Test3()
        {
            var services = new Cat()
                .Register<Base,Foo>(LifeTime.Transient)
                .Register<Base,Bar>(LifeTime.Transient)
                .Register<Base,Baz>(LifeTime.Transient)
                .GetServices<Base>();
            Console.WriteLine("注册后的实例");
            foreach (var baBase in services)
            {
                Console.WriteLine(baBase);
            }
            Debug.Assert(services.OfType<Foo>().Any());
            Debug.Assert(services.OfType<Bar>().Any());
            Debug.Assert(services.OfType<Baz>().Any());
        }

        private static void Test4()
        {
            using (var root = new Cat()
                .Register<IFoo, Foo>(LifeTime.Transient)
                .Register<IBar>(_ => new Bar(), LifeTime.Self)
                .Register<IBaz, Baz>(LifeTime.Root)
                .Register(Assembly.GetEntryAssembly()))
            {
                Console.WriteLine("root注册后的字典内容");
                foreach (var keyValuePair in root._registries)
                {
                    Console.WriteLine(keyValuePair.Key + "" + keyValuePair.Value);
                }

                using (var cat = root.CreateChild())
                {
                    cat.GetService<IFoo>();
                    cat.GetService<IBaz>();
                    cat.GetService<IBar>();
                    cat.GetService<IQux>();
                    Console.WriteLine("cat注册后的字典内容");
                    foreach (var keyValuePair in cat._registries)
                    {
                        Console.WriteLine(keyValuePair.Key + "" + keyValuePair.Value);
                    }
                    Console.WriteLine("Child cat is disposed.");
                }

                Console.WriteLine("Root cat is disposed.");
            }
        }
    }
}