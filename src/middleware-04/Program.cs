using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace middleware_04
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
                    app.Map("/monkey", _ =>
                    {
                        _.Use(async (context, next) =>
                        {
                            await context.Response.WriteAsync("Monkey path was called\n");
                        });
                    });

                    app.Map("/dog", _ =>
                    {
                        _.Map("/snoop", __ => {
                            __.Run(async context =>
                            {
                                await context.Response.WriteAsync("dog/snoop path was called\n");
                            });
                        });

                        _.Map("/eatdog", __ => {
                            __.Run(async context =>
                            {
                                await context.Response.WriteAsync("dog/eatdog path was called\n");
                            });
                        });

                        _.Run(async context =>
                        {
                            await context.Response.WriteAsync("dog path was called\n");
                        });
                    });

                    app.Run(async (context) =>
                    {
                        await context.Response.WriteAsync("some other path was called\n");
                    });
                });
    }
}
