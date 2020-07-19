using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Model;

namespace Factory
{
    public interface IControllerExecutor
    {
        Task<View> ExecuteAsync(Controller controller, HttpContext httpContext);
    }
}