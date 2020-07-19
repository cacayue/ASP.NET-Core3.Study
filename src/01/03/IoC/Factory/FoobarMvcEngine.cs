using System.Threading.Tasks;
using Model;

namespace Factory
{
    public class FoobarMvcEngine: MvcEngine
    {
        protected override IControllerActivator GetControllerActivator()
        {
            return new SingletonControllerActivator();
        }
    }
}