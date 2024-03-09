using Syki.Back.CreateUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using static Syki.Back.Configs.AuthorizationConfigs;
using Syki.Back.CreateInstitution;

namespace Syki.Tests.Base;

[Category("Integration")]
public class IntegrationTestBase
{
    protected BackWebApplicationFactory _factory = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _factory = new BackWebApplicationFactory();
        using var scope = _factory.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        await ctx.ResetDbAsync();

        ctx.Add(new Faculdade { Id = Guid.Empty, Nome = "Syki" });
        await ctx.SaveChangesAsync();

        var user = new SykiUser(Guid.Empty, "Syki Adm", "adm@syki.com");
        var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<SykiUser>>();
        await _userManager.CreateAsync(user, "Admin@123Admin@123");
        await _userManager.AddToRoleAsync(user, Adm);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _factory.DisposeAsync();
    }
}
