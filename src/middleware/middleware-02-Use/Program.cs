using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace middleware_02
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .Configure(app =>
                {
                    app.Use(async (context, next) =>
                    {
                        await context.Response.WriteAsync($"The content from the first middleware\n");
                        await next.Invoke();
                    });

                    app.Use(async (context, next) =>
                    {
                        await context.Response.WriteAsync($"The content from the second middleware\n");
                    });
                });
    }
}
