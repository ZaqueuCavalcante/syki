using Exato.Back.Converters;
using Exato.Back.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;

namespace Exato.Back.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new ExatoStringEnumConverter()));

        builder.Services.Configure<MvcOptions>(options =>
        {
            options.Filters.Add(new ProducesAttribute("application/json"));
            options.Filters.Add(new ConsumesAttribute("application/json"));
        });

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        builder.Services.AddHttpClient();
    }

    public static void UseApiPrefix(this IApplicationBuilder app)
    {
        app.UsePathBase("/api");
    }

    public static void UseExceptions(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionsMiddleware>();
    }

    public static void UseControllers(this IApplicationBuilder app)
    {
        app.UseEndpoints(options =>
        {
            options.MapControllers();

            options.MapOpenApi();
            options.MapScalarDocs();
        });
    }

    public static void UseLogs(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
    }

    public static void UseHttpsConfigs(this IApplicationBuilder app)
    {
        var fwd = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost,
            RequireHeaderSymmetry = false,
            ForwardLimit = null,
        };

        app.UseForwardedHeaders(fwd);
        app.UseHsts();
        app.UseHttpsRedirection();
    }
}
