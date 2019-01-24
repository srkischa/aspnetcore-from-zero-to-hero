using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace middleware_06
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
                    appBuilder.UseMiddleware<ConventionalMiddleware>();
                    appBuilder.UseMiddleware<FactoryBasedMiddleware>();
                    appBuilder.Run(async (context) => await context.Response.WriteAsync($"Last part of the response\n"));
                })
            .ConfigureServices(services =>
            {
                services.AddTransient<FactoryBasedMiddleware>();
                services.AddScoped<IScopedService, ScopedService>();
            });
    }

    public class ConventionalMiddleware
    {
        private readonly RequestDelegate _next;

        public ConventionalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IScopedService service)
        {
            await httpContext.Response.WriteAsync($"First part of the response, length = {service.Length.ToString()}\n");
            service.Length += 130;
            await _next(httpContext);
        }
    }

    public class FactoryBasedMiddleware: IMiddleware
    {
        private readonly IScopedService _service;

        public FactoryBasedMiddleware(IScopedService service)
        {
            _service = service;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync($"Second part of the response, length = {_service.Length.ToString()}\n");
            await next(context);
        }
    }

    public interface IScopedService
    {
        int Length { get; set;  }
    }

    public class ScopedService: IScopedService
    {
        public int Length { get; set; } = 20;
    }
}
