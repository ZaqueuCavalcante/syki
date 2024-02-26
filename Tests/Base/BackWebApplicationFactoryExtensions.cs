using Syki.Back.Database;
using Syki.Front.FinishUserRegister;
using Syki.Front.CreatePendingUserRegister;
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








    public static HttpClient Http(this BackWebApplicationFactory factory)
    {
        return factory.CreateClient();
    }

    public static async Task<HttpClient> LoggedAsAcademico(this BackWebApplicationFactory factory)
    {
        var client = factory.CreateClient();
        var faculdade = await client.CreateInstitution();
        await client.RegisterAndLogin(faculdade.Id, Academico);
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

    public static async Task<string?> GetResetPasswordToken(this BackWebApplicationFactory factory, Guid userId)
    {
        using var ctx = factory.GetDbContext();

        var id = await ctx.ResetPasswords
            .Where(r => r.UserId == userId && r.UsedAt == null)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        return id == Guid.Empty ? null : id.ToString();
    }
}
