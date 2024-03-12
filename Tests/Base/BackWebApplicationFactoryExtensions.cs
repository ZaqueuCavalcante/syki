using Microsoft.Extensions.DependencyInjection;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Base;

public static class BackWebApplicationFactoryExtensions
{
    public static HttpClient GetClient(this BackWebApplicationFactory factory)
    {
        return factory.CreateClient();
    }

    public static async Task<string?> GetRegisterSetupToken(this BackWebApplicationFactory factory, string email)
    {
        using var ctx = factory.GetDbContext();
        var register = await ctx.UserRegisters.FirstOrDefaultAsync(d => d.Email == email);
        return register?.Id.ToString();
    }

    public static async Task<HttpClient> LoggedAsAcademico(this BackWebApplicationFactory factory)
    {
        var client = factory.GetClient();
        var user = await client.RegisterUser(factory);
        await client.Login(user.Email, user.Password);
        return client;
    }

    public static SykiDbContext GetDbContext(this BackWebApplicationFactory factory)
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<SykiDbContext>();
    }

    public static T GetService<T>(this BackWebApplicationFactory factory) where T : notnull
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }

    public static async Task<string?> GetResetPasswordToken(this BackWebApplicationFactory factory, string email)
    {
        using var ctx = factory.GetDbContext();

        var user = await ctx.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
            return null;

        var id = await ctx.ResetPasswordTokens
            .Where(r => r.UserId == user.Id && r.UsedAt == null)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        return id == Guid.Empty ? null : id.ToString();
    }
}
