using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Base;

public static class SykiWebApplicationFactoryExtensions
{
    public static async Task<HttpClient> LoggedAsAcademico(this SykiWebApplicationFactory factory)
    {
        var client = factory.CreateClient();
        var faculdade = await client.CreateFaculdade();
        await client.RegisterAndLogin(faculdade.Id, Academico);
        return client;
    }

    public static SykiDbContext GetDbContext(this SykiWebApplicationFactory factory)
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<SykiDbContext>();
    }

    public static T GetService<T>(this SykiWebApplicationFactory factory) where T : notnull
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }

    public static async Task<string?> GetDemoSetupToken(this SykiWebApplicationFactory factory, string email)
    {
        using var ctx = factory.GetDbContext();
        var demo = await ctx.Demos.FirstOrDefaultAsync(d => d.Email == email);
        return demo?.Id.ToString();
    }
}
