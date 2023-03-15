using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Extensions.Middlewares.Plugin
{
    public class BasePlugin : IPlugin
    {
        public virtual string PluginName => Assembly.GetExecutingAssembly().GetName().Name ?? "Unknown Plugin";

        public virtual string PluginVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown Version";

        public virtual string? PluginDescription => null;

        public virtual string Author => "Unknown Author";

        public virtual string? ProjectUrl => null;

        public void Enable(IApplicationBuilder app)
        {
            OnPluginStart(app);
        }

        public void Disable()
        {
            OnPluginStop();
        }

        public virtual void OnPluginStart(IApplicationBuilder app)
        {

        }

        public virtual void OnPluginStop()
        {

        }
    }
}
