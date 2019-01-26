using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace middleware_03
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

                    //The Run terminates response (short circuit)
                    app.Run(async (context) => await context.Response.WriteAsync($"The content from the second middleware\n"));

                    app.Use(async (context, next) =>
                    {
                        await context.Response.WriteAsync($"This will not be called\n");
                        await next.Invoke();
                    });
                });
    }
}
