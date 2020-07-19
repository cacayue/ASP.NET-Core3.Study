using System;
using System.Threading.Tasks;

namespace AbstractFactory
{
    public class App
    {
        static async Task Main()
        {
            var address = new Uri("");
            var engine = new MvcEngine(new FoobarMvcEngineFactory());
            await engine.StartAsync(address);
            //TODO
        }
    }
}