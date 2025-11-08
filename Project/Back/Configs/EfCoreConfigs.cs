using Npgsql;
using Exato.Web;
using Exato.Back.Middlewares;

namespace Exato.Back.Configs;

public static class EfCoreConfigs
{
    public static void AddEfCoreConfigs(this WebApplicationBuilder builder)
    {
        var database = builder.Configuration.Database;

        builder.Services.AddSingleton(sp =>
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(database.Exato);

            dataSourceBuilder.ConfigureTracing(x =>
            {
                x.ConfigureCommandSpanNameProvider(provider =>
                {
                    return provider.CommandText.GetSqlSpanName();
                });
            });

            return dataSourceBuilder.Build();
        });

        builder.Services.AddDbContext<BackDbContext>();

        builder.Services.AddDbContext<WebDbContext>(options => options.UseNpgsql(database.Web));
    }

    public static void UseEnrichDbContext(this IApplicationBuilder app)
    {
        app.UseMiddleware<EnrichBackDbContextMiddleware>();
    }
}
