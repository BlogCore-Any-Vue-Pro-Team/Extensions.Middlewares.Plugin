using Microsoft.AspNetCore.Builder;

namespace Extensions.Middlewares.Plugin
{
    public class BasePlugin : IPlugin
    {
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
