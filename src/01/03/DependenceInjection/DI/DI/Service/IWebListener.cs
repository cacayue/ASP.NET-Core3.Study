using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Model;

namespace Factory
{
    public interface IWebListener
    {
        Task ListenAsync(Uri address);
        Task<HttpContext> ReceiveAsync();
    }
}