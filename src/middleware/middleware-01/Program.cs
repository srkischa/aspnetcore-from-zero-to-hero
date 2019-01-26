using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
 
namespace authorization
{
    public class middleware_01
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .Configure(app =>
                {
                    //The order of these things is important. 
                    app.Run(async (context) =>
                    {
                        await context.Response.WriteAsync($"Run will be called but use will not\n");
                    });

                    app.Use(async (context, next) =>
                    {
                        await context.Response.WriteAsync($"This will not be called\n");
                        await next.Invoke();
                    });
                });
    }
}
