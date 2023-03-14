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

        public bool Enable(IApplicationBuilder app)
        {
            OnPluginStart(app);
            return true;
        }

        public bool Disable()
        {
            OnPluginStop();
            return true;
        }

        public virtual void OnPluginStart(IApplicationBuilder app)
        {

        }

        public virtual void OnPluginStop()
        {

        }
    }
}
