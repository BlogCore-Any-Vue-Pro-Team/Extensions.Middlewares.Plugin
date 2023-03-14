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
        var plugin_types = plugin_assembly.GetExportedTypes().Where(exported_type => exported_type.GetInterfaces().Contains(typeof(IPlugin)));
        if (!plugin_types.Any())
        {
            throw new EntryPointNotFoundException();
        }
        var plugin_objects = plugin_types.Select(plugin_type => Activator.CreateInstance(plugin_type) as IPlugin ?? throw new InvalidOperationException());
        app
            .Services
            .GetRequiredService<RuntimeMiddlewareService>()
            .Configure(
                app_builder => {
                    app_builder.Map(
                        $"/{ComputeAssemblyHash(plugin_assembly)}",
                        app_builder => {
                            var results = plugin_objects.Select(plugin_object => plugin_object.Enable(app_builder));
                            // IMPORTANT!
                            // MIDDLEWARES MAY NOT BE ADDED WITHOUT THIS JUDGEMENT!
                            // I'm curious about the reason.
                            if (results.Any(result => result == false))
                            {
                                throw new InvalidOperationException();
                            }
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
