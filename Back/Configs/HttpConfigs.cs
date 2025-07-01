using Syki.Back.Hubs;
using System.Text.Json.Serialization;

namespace Syki.Back.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        builder.Services.AddSignalR();
        
        builder.Services.AddHttpClient();
    }

    public static void UseExceptions(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionsMiddleware>();
    }

    public static void UseCustomHeaders(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomHeadersMiddleware>();
    }

    public static void UseControllers(this IApplicationBuilder app)
    {
        app.UseEndpoints(options =>
        {
            options.MapControllers();

            options.MapHub<SykiHub>("/syki-hub");

            options.MapOpenApi();
            options.MapScalarDocs();
        });
    }

    public static void UseLogs(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
    }
}
