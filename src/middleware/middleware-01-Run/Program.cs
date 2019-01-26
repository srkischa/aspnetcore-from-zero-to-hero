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
                    //The run delegate terminates the pipeline 
                    app.Run(async (context) =>
                    {
                        await context.Response.WriteAsync($"This Run will be called but second Run will not\n");
                    });

                    //This will not be called
                    app.Run(async (context) =>
                    {
                        await context.Response.WriteAsync($"This will not be called\n");
                    });
                });
    }
}
