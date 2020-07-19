using System;
using Factory;

namespace AbstractFactory
{
    public interface IMvcEngineFactory
    {
        IWebListener GetWebListener();
        IControllerActivator GetControllerActivator();
        IControllerExecutor GetControllerExecutor();
        IViewRenderer GetViewRenderer();
    }
}
