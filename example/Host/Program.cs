using Extensions.Middlewares.Plugin;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRuntimeMiddleware();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRuntimeMiddleware();
app.MapGet(
    "/register-plugin",
    () => {
        Assembly plugin_assembly = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugin.dll"));
        Type plugin_type = plugin_assembly.GetType("Plugin.Class") ?? throw new EntryPointNotFoundException();
        object plugin_object = Activator.CreateInstance(plugin_type) ?? throw new InvalidOperationException();
        MethodInfo register_middleware_method = plugin_type.GetMethod("RegisterMiddleware") ?? throw new NotImplementedException();
        app
            .Services
            .GetRequiredService<RuntimeMiddlewareService>()
            .Configure(
                app_builder => {
                    app_builder.Map(
                        $"/{ComputeAssemblyHash(plugin_assembly)}",
                        app_builder => {
                            register_middleware_method.Invoke(plugin_object, new object[] { app_builder });
                        }
                    );
                }
            );
    }
);

app.Run();

static string ComputeAssemblyHash(Assembly assembly)
{
    using (var hash_algorithm = SHA256.Create())
    {
        byte[] hash = hash_algorithm.ComputeHash(File.ReadAllBytes(assembly.Location));

        StringBuilder string_builder = new StringBuilder();
        foreach (byte b in hash)
        {
            string_builder.Append(b.ToString("x2"));
        }

        return string_builder.ToString();
    }
}
