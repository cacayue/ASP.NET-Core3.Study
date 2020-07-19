using System;
using System.Threading.Tasks;

namespace AbstractFactory
{
    public class MvcEngine
    {
        public IMvcEngineFactory EngineFactory { get; }

        public MvcEngine(IMvcEngineFactory mvcEngineFactory = null)
        {
            EngineFactory = mvcEngineFactory ?? new MvcEngineFactory();
        }

        public async Task StartAsync(Uri address)
        {
            var listener = EngineFactory.GetWebListener();
            var activator = EngineFactory.GetControllerActivator();
            var executor = EngineFactory.GetControllerExecutor();
            var render = EngineFactory.GetViewRenderer();

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