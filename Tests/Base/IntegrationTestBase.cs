using NUnit.Framework;
using Syki.Back.Domain;
using Syki.Back.Database;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Base;

[Category("Integration")]
public class IntegrationTestBase
{
    protected SykiWebApplicationFactory _factory = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _factory = new SykiWebApplicationFactory();

        using var scope = _factory.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        if (Env.IsTesting())
        {
            await ctx.Database.EnsureDeletedAsync();
            await ctx.Database.EnsureCreatedAsync();
            // Throw.DE0000.Now();
        }

        ctx.Add(new Faculdade { Id = Guid.Empty, Nome = "Syki" });
        await ctx.SaveChangesAsync();

        var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<SykiUser>>();

        var user = new SykiUser(Guid.Empty, "Syki Adm", "adm@syki.com");

        await _userManager.CreateAsync(user, "Adm@123");
        await _userManager.AddToRoleAsync(user, Adm);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _factory.DisposeAsync();
    }
}
