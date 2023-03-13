using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Middlewares.Plugin
{
    public static class RuntimeMiddlewareExtensions
    {
        public static IServiceCollection AddRuntimeMiddleware(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            services.Add(new ServiceDescriptor(typeof(RuntimeMiddlewareService), typeof(RuntimeMiddlewareService), lifetime));
            return services;
        }

        public static IApplicationBuilder UseRuntimeMiddleware(this IApplicationBuilder app, Action<IApplicationBuilder>? default_action = null)
        {
            var service = app.ApplicationServices.GetRequiredService<RuntimeMiddlewareService>();
            service.Use(app);
            if (default_action != null)
            {
                service.Configure(default_action);
            }
            return app;
        }
    }
}
