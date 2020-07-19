using System;
using System.Threading.Tasks;
using Model;

namespace Factory
{
    public class MvcEngine
    {
        public async Task StartAsync(Uri address)
        {
            var listener = GetWebListener();
            var activator = GetControllerActivator();
            var executor = GetControllerExecutor();
            var render = GetViewRenderer();

            await listener.ListenAsync(address);
            while (true)
            {
                var httpContext = await listener.ReceiveAsync();
                var controller = await activator.CreateControllerAsync(httpContext);
                try
                {
                    var view = await executor.ExecuteAsync(controller,httpContext);
                    await render.RendAsync(view,httpContext);
                }
                finally
                {
                    await activator.ReleaseAsync(controller);
                }
            }
        }

        protected virtual IWebListener GetWebListener()
        {
            return null;
        }

        protected virtual IControllerActivator GetControllerActivator()
        {
            return null;
        }
        protected virtual IControllerExecutor GetControllerExecutor()
        {
            return null;
        }
        protected virtual IViewRenderer GetViewRenderer()
        {
            return null;
        }
    }
}
