using System.Threading.Tasks;
using Model;

namespace TemplateMethod
{
    public class FoobarMvcEngine: MvcEngine
    {
        protected override Task<Controller> CreateControllerAsync(Request request)
        {
            var a = 1 + 1;
            return base.CreateControllerAsync(request);
        }
    }
}