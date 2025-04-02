using Scalar.AspNetCore;
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

            options.MapOpenApi();
            options.MapScalarApiReference(options =>
            {
                options.WithModels(false);
                options.WithDownloadButton(false);
                options.WithTitle("Syki API Docs");
                options.WithEndpointPrefix("/docs/{documentName}");
                options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
                options
                    .WithPreferredScheme("Bearer")
                    .WithHttpBearerAuthentication(bearer =>
                    {
                        bearer.Token = "your.bearer.token";
                    })
                    .WithHttpBasicAuthentication(basic => {});
            });
        });
    }

    public static void UseLogs(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
    }
}
