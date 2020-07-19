using Factory;

namespace AbstractFactory
{
    public class FoobarMvcEngineFactory:MvcEngineFactory
    {
        public override IControllerActivator GetControllerActivator()
        {
            return new SingletonControllerActivator();
        }
    }
}