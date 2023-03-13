using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Extensions.Middlewares.Plugin
{
    internal class RuntimeMiddlewareService
    {
        private IApplicationBuilder? app_builder_;
        private Func<RequestDelegate, RequestDelegate>? middleware_;

        internal void Use(IApplicationBuilder app)
        {
            app_builder_ = app.Use(next => context => middleware_ == null ? next(context) : middleware_(next)(context));
        }

        public void Configure(Action<IApplicationBuilder> action)
        {
            if (app_builder_== null)
            {
                throw new InvalidOperationException();
            }

            var app = app_builder_.New();
            action(app);
            middleware_ = next => app.Use(_ => next).Build();
        }
    }
}
