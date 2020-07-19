using System;
using System.Threading.Tasks;
using Model;

namespace TemplateMethod
{
    public class MvcEngine
    {

        public async Task StartAsync(Uri address)
        {
            await ListenAsync(address);
            while (true)
            {
                var request = await ReceiveAsync();
                var controller = await CreateControllerAsync(request);
                var view = await ExecuteControllerAsync(controller);
                await RenderViewAsync(view);
            }
        }

        protected virtual Task ListenAsync(Uri address)
        {
            return Task.CompletedTask;
        }

        protected virtual Task<Request> ReceiveAsync()
        {
            return Task.Run(() => new Request());
        }

        protected virtual Task<Controller> CreateControllerAsync(Request request)
        {
            return Task.Run(() => new Controller());
        }

        protected virtual Task<View> ExecuteControllerAsync(Controller controller)
        {
            return Task.Run(() => new View());
        }

        protected virtual Task RenderViewAsync(View view)
        {
            return Task.CompletedTask;
        }
    }
}
