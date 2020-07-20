using System;
using System.Threading.Tasks;
using DI;
using Factory;

namespace AbstractFactory
{
    public class MvcEngine
    {
        public Cat Cat { get; }

        public MvcEngine(Cat cat)
        {
            Cat = cat;
        }

        public async Task StartAsync(Uri address)
        {
            var listener = Cat.GetService<IWebListener>();
            var activator = Cat.GetService<IControllerActivator>();
            var executor = Cat.GetService<IControllerExecutor>();
            var render = Cat.GetService<IViewRenderer>();

            await listener.ListenAsync(address);
            while (true)
            {
                var httpContext = await listener.ReceiveAsync();
                var controller = await activator.CreateControllerAsync(httpContext);
                try
                {
                    var view = await executor.ExecuteAsync(controller, httpContext);
                    await render.RendAsync(view, httpContext);
                }
                finally
                {
                    await activator.ReleaseAsync(controller);
                }
            }
        }
    }
}