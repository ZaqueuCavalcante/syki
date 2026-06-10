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
                .WithOrigins(
                    "http://localhost:3000",
                    "https://app.syki.com.br",
                    "https://estud.up.railway.app"
                )
            )
        );
    }
}
