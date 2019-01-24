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
                        await context.Response.WriteAsync($"This will be called 1\n");
                        await next.Invoke();
                    });

                    //The order of these things is important. 
                    app.Run(async (context) => await context.Response.WriteAsync($"This will be called 2\n"));

                    app.Use(async (context, next) =>
                    {
                        await context.Response.WriteAsync($"This will not be called\n");
                        await next.Invoke();
                    });
                });
    }
}
