using Microsoft.AspNetCore.Builder;

namespace Extensions.Middlewares.Plugin
{
    public interface IPlugin
    {
        public string PluginName { get; }

        public string PluginVersion { get; }

        public string? PluginDescription { get; }

        public string Author { get; }

        public string? ProjectUrl { get; }

        public void Enable(IApplicationBuilder app);

        public void Disable();
    }
}
