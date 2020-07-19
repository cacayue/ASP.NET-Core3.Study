using Factory;

namespace AbstractFactory
{
    public class MvcEngineFactory : IMvcEngineFactory
    {
        public virtual IControllerActivator GetControllerActivator()
        {
            throw new System.NotImplementedException();
        }

        public virtual IControllerExecutor GetControllerExecutor()
        {
            throw new System.NotImplementedException();
        }

        public virtual IViewRenderer GetViewRenderer()
        {
            throw new System.NotImplementedException();
        }

        public virtual IWebListener GetWebListener()
        {
            throw new System.NotImplementedException();
        }
    }
}