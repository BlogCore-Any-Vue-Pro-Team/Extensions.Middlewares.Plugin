using Microsoft.AspNetCore.Builder;

namespace Extensions.Middlewares.Plugin
{
    public interface IPlugin
    {
        public bool Enable(IApplicationBuilder app);

        public bool Disable();
    }
}
