using System;
using System.Threading.Tasks;

namespace Mvc
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                var address = new Uri("http://0.0.0.0:8080/mvcapp");
                await MvcLib.MvcLib.ListenAsync(address);
                while (true)
                {
                    var request = await MvcLib.MvcLib.ReceiveAsync();
                    var controller = await MvcLib.MvcLib.CreateControllerAsync(request);
                    var view = await MvcLib.MvcLib.ExecuteControllerAsync(controller);
                    await MvcLib.MvcLib.RenderViewAsync(view);
                }
            }
        }
    }
}
