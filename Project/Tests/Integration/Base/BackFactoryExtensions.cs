using Exato.Web;
using Exato.Shared.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Exato.Back.Intelligence.Domain.Public;
using Microsoft.Extensions.DependencyInjection;
using Exato.Back.Features.Cross.CreateExatoUser;
using Exato.Shared.Features.Office.CriarEmpresa;

namespace Exato.Tests.Integration.Base;

public static class BackFactoryExtensions
{
    public static HttpClient GetClient(this BackFactory factory)
    {
        return factory.CreateClient();
    }

    public static async Task RegisterExatoAdm(this BackFactory factory)
    {
        await using var ctx = factory.GetBackDbContext();
        using var userManager = factory.GetUserManager();

        ctx.Operation = nameof(RegisterExatoAdm);
        ctx.ActivityId = Guid.CreateVersion7().ToString();

        var organization = new Cliente(
            "Exato Digital",
            "Exato Digital LTDA.",
            "12387530000113"
        );
        await ctx.SaveChangesAsync(organization);
        IntegrationTestBase.ExatoId = organization.ClienteId;
        IntegrationTestBase.ExatoExternalId = organization.ExternalId;

        var adminRole = new ExatoRole("OfficeAdm", "OfficeAdm", organization.ClienteId, ExatoFeaturesStore.Features.ConvertAll(x => x.Id));
        await ctx.SaveChangesAsync(adminRole);
        IntegrationTestBase.ExatoAdminRoleId = adminRole.Id;

        var csRole = new ExatoRole("OfficeCustomerSuccess", "OfficeCustomerSuccess", organization.ClienteId, [14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,35,36,37]);
        await ctx.SaveChangesAsync(csRole);
        IntegrationTestBase.ExatoCSRoleId = csRole.Id;

        var user = new ExatoUser(
            organization.ClienteId,
            IntegrationTestBase.ExatoAdmName,
            IntegrationTestBase.ExatoAdmEmail
        );
        await userManager.CreateAsync(user, IntegrationTestBase.ExatoAdmPassword);

        var userRole = new ExatoUserRole(user.Id, adminRole.Id);
        await ctx.SaveChangesAsync(userRole);
    }

    public static async Task<OfficeHttpClient> LoggedAsExatoAdm(this BackFactory factory)
    {
        var client = factory.GetClient();
        await client.Login(IntegrationTestBase.ExatoAdmEmail, IntegrationTestBase.ExatoAdmPassword);
        return new(client);
    }

    public static async Task<OfficeHttpClient> LoggedAsOfficeCS(this BackFactory factory)
    {
        var exatoAdmClient = await factory.LoggedAsExatoAdm();
        var org = new CriarEmpresaOut { Id = IntegrationTestBase.ExatoId, ExternalId = IntegrationTestBase.ExatoExternalId };
        var user = (await exatoAdmClient.CreateUser(org.Id, DataGen.UserName, DataGen.Email, IntegrationTestBase.ExatoCSRoleId)).Success;

        var email = user.Email;
        var password = IntegrationTestBase.ExatoCSPassword;

        var client = factory.GetClient();

        await client.SendResetPasswordToken(email);

        var token = await factory.GetResetPasswordToken(email);

        await client.ResetPassword(token!, password);
        await client.Login(email, password);

        return new(client) { Org = org, User = user };
    }

    public static BackDbContext GetBackDbContext(this BackFactory factory)
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<BackDbContext>();
    }

    public static WebDbContext GetWebDbContext(this BackFactory factory)
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<WebDbContext>();
    }

    public static UserManager<ExatoUser> GetUserManager(this BackFactory factory)
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<UserManager<ExatoUser>>();
    }

    public static async Task<string?> GetResetPasswordToken(this BackFactory factory, string email)
    {
        await using var ctx = factory.GetBackDbContext();

        var user = await ctx.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
            return null;

        var id = await ctx.ExatoResetPasswordTokens
            .Where(r => r.UserId == user.Id && r.UsedAt == null)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        return id == Guid.Empty ? null : id.ToString();
    }
}
