using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Extensions.Middlewares.Plugin
{
    public class RuntimeMiddlewareService
    {
        private IApplicationBuilder? app_builder_;
        private Func<RequestDelegate, RequestDelegate>? middleware_;

        internal void Use(IApplicationBuilder app)
        {
            app_builder_ = app.Use(
                (next) => {
                    return (context) => {
                        return middleware_ == null ? next(context) : middleware_(next)(context);
                    };
                }
            );
        }

        public void Configure(Action<IApplicationBuilder> action)
        {
            if (app_builder_== null)
            {
                throw new InvalidOperationException();
            }

            var new_app_builder = app_builder_.New();
            action(new_app_builder);
            middleware_ = next => new_app_builder.Use(_ => next).Build();
        }
    }
}
