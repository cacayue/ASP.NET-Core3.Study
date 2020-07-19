using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Model;

namespace Factory
{
    public class SingletonControllerActivator:IControllerActivator
    {
        public Task<Controller> CreateControllerAsync(HttpContext httpContext)
        {
            throw new System.NotImplementedException();
        }

        public Task ReleaseAsync(Controller controller)
        {
            throw new System.NotImplementedException();
        }
    }
}