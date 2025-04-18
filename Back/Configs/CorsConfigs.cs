namespace Syki.Back.Configs;

public static class CorsConfigs
{
    public static void AddCorsConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => options
            .AddDefaultPolicy(builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("X-Hash", "X-CrossLogin")
                .SetIsOriginAllowed(_ => true)
            )
        );
    }
}
