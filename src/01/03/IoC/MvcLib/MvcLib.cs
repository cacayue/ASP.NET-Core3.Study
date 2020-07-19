
using System;
using System.Threading.Tasks;
using Model;

namespace MvcLib
{
    public static class MvcLib
    {
        public static Task ListenAsync(Uri address)
        {
            return Task.CompletedTask;
        }

        public static Task<Request> ReceiveAsync()
        {
            return Task.Run(() => new Request());
        }

        public static Task<Controller> CreateControllerAsync(Request request)
        {
            return Task.Run(() => new Controller());
        }

        public static Task<View> ExecuteControllerAsync(Controller controller)
        {
            return Task.Run(() => new View());
        }

        public static Task RenderViewAsync(View view)
        {
            return Task.CompletedTask;
        }
    }
}
