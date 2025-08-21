using Syki.Back.Hubs;
using Syki.Back.Converters;

namespace Syki.Back.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new SykiStringEnumConverter()));

        builder.Services.Configure<MvcOptions>(options =>
        {
            options.Filters.Add(new ProducesAttribute("application/json"));
            options.Filters.Add(new ConsumesAttribute("application/json"));
        });

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
