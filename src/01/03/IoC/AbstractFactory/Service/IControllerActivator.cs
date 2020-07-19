using System.Threading.Tasks;
using Model;
using Microsoft.AspNetCore.Http;

namespace Factory
{
    public interface IControllerActivator
    {
        Task<Controller> CreateControllerAsync(HttpContext httpContext);
        Task ReleaseAsync(Controller controller);
    }
}