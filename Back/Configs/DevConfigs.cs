namespace Syki.Back.Configs;

public static class DevConfigs
{
    public static void AddDevConfigs(this WebApplication app)
    {
        if (!Env.IsDevelopment()) return;

        var settings = new DatabaseSettings(app.Configuration);
        if (settings.Reset)
        {
            app.Services.CreateScope().ServiceProvider.GetRequiredService<SykiDbContext>().ResetDevDb(); 
        }
    }
}
