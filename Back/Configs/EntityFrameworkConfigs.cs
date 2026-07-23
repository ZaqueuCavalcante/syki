using Npgsql;

namespace Estud.Back.Configs;

public static class EntityFrameworkConfigs
{
    public static void AddEntityFrameworkConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(sp =>
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.Database.ConnectionString);

            dataSourceBuilder.ConfigureTracing(x =>
            {
                x.ConfigureCommandSpanNameProvider(provider =>
                {
                    return provider.CommandText.GetSqlSpanName();
                });
            });

            return dataSourceBuilder.Build();
        });

        builder.Services.AddDbContext<EstudDbContext>();
    }

    public static void UseEnrichDbContext(this IApplicationBuilder app)
    {
        app.UseMiddleware<EnrichBackDbContextMiddleware>();
    }

    public static void UseBackgroundProcessorsTrigger(this IApplicationBuilder app)
    {
        app.UseMiddleware<BackgroundProcessorsTriggerMiddleware>();
    }
}
