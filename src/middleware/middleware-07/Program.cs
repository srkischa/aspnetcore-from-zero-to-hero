using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace middleware_07
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .Configure(appBuilder =>
                {
                    appBuilder.UseMiddleware<CustomMiddleware1>();
                    appBuilder.UseMiddleware<CustomMiddleware2>();
                    appBuilder.Run(async (context) => await context.Response.WriteAsync($"Last part of the response\n"));
                })
            .ConfigureServices(services =>
            {
                services.AddScoped<IScopedService, ScopedService>();
            });
    }

    public class CustomMiddleware1
    {
        private readonly RequestDelegate _next;

        public CustomMiddleware1(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IScopedService service)
        {
            service.Length = 20;
            await httpContext.Response.WriteAsync($"First part of the response\n");
            await _next(httpContext);
        }
    }

    public class CustomMiddleware2
    {
        private readonly RequestDelegate _next;

        public CustomMiddleware2(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IScopedService service)
        {
            await httpContext.Response.WriteAsync($"Second part of the response, value = {service.Length.ToString()}\n");
            await _next(httpContext);
        }
    }

    public interface IScopedService
    {
        int Length { get; set; }
    }

    public class ScopedService : IScopedService
    {
        public int Length { get; set; }
    }
}
