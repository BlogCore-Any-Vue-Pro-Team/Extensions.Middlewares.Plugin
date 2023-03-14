using Extensions.Middlewares.Plugin;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Plugin
{
    public class Class : BasePlugin
    {
        public override void OnPluginStart(IApplicationBuilder app)
        {
            app.Map(
                "/plugin-page",
                (app_builder) => {
                    app_builder.Run(
                        async (context) => {
                            await context.Response.WriteAsync("Hello, world!");
                        }
                    );
                }
            );
        }
    }
}
