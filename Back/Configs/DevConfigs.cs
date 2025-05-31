namespace Syki.Back.Configs;

public static class DevConfigs
{
    public static void AddDevConfigs(this WebApplication app)
    {
        if (!Env.IsDevelopment()) return;

        var ctx = app.Services.CreateScope().ServiceProvider.GetRequiredService<SykiDbContext>();

        var settings = new DatabaseSettings(app.Configuration);
        if (settings.Reset)
        {
            ctx.ResetDevDb();
        }
        else
        {
            ctx.Database.Migrate();
        }
    }
}
