using Npgsql;

namespace Syki.Back.Configs;

public static class EfCoreConfigs
{
    public static void AddEfCoreConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(sp =>
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.Database().ConnectionString);

            dataSourceBuilder.ConfigureTracing(x =>
            {
                x.ConfigureCommandSpanNameProvider(provider =>
                {
                    return provider.CommandText.GetSqlSpanName();
                });
            });

            return dataSourceBuilder.Build();
        });

        builder.Services.AddDbContext<SykiDbContext>();
    }
}
