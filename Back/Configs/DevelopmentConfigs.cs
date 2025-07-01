namespace Syki.Back.Configs;

public static class DevelopmentConfigs
{
    public static void RunDevelopmentConfigs(this WebApplication app)
    {
        if (!Env.IsDevelopment()) return;

        var ctx = app.Services.CreateScope().ServiceProvider.GetRequiredService<SykiDbContext>();

        var settings = app.Configuration.Database();
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
