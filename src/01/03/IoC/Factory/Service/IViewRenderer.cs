using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Model;

namespace Factory
{
    public interface IViewRenderer
    {
        Task RendAsync(View view, HttpContext httpContext);
    }
}