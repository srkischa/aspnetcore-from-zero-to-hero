using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace middleware_05
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
                    appBuilder.MapWhen(context => context.Request.Path.StartsWithSegments("/api"), innerBuilder =>
                    {
                        innerBuilder.Run(async (context) =>
                        {
                            await context.Response.WriteAsync(JsonConvert.SerializeObject($"Hello from the api side"));
                        });
                    });

                    appBuilder.MapWhen(context => context.Request.Path.StartsWithSegments("/assets"), innerBuilder =>
                    {
                        innerBuilder.UseStaticFiles();
                    });

                    appBuilder.Run(context =>
                    {
                        context.Response.Headers.Add("content-type", "text/html");
                        return context.Response.WriteAsync(@"
                            <div style='background-image:url(../assets/Magic.gif);background-repeat: no-repeat;background-position: center; height:400px;'>
                                <span>Hello World</span>
                            </div>
                            <script>
                             fetch('api')
                              .then((response) => response.json())
                              .then((json) => {
                                var span = document.getElementsByTagName('span')[0];
                                span.innerHTML = json;
                              });
                            </script>
                        ");
                    });
                });
    }
}
