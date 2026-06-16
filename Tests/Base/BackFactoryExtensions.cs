using Quartz;
using Syki.Back.Emails;
using Syki.Back.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Syki.Tests.Integration.Clients;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Syki.Tests.Base;

public static class BackFactoryExtensions
{
    public static TestsHttpClient GetTestsClient(this BackFactory factory)
    {
        return new TestsHttpClient(factory.CreateClient());
    }

    public static TestsHttpClient GetNoRedirectTestsClient(this BackFactory factory)
    {
        var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false };
        return new TestsHttpClient(factory.CreateClient(options));
    }

    public static ISchedulerFactory GetSchedulerFactory(this BackFactory factory)
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ISchedulerFactory>();
    }

    public static async Task AwaitCommandsProcessing(this BackFactory factory)
    {
        await using var ctx = factory.GetDbContext();

        var scheduler = await factory.GetSchedulerFactory().GetScheduler();
        await scheduler.TriggerCommandsProcessorJob();

        var count = 0;
        while (true)
        {
            if (count == 5) break;

            var commands = await ctx.Commands.CountAsync(x => x.ProcessedAt == null);
            if (commands == 0) break;
            await Task.Delay(500);
            count ++;
        }
    }

    public static async Task<string?> GetMagicLink(this BackFactory factory, string email)
    {
        await using var ctx = factory.GetDbContext();

        var user = await ctx.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
        if (user == null) return null;

        var id = await ctx.WebMagicLinks
            .Where(t => t.UserId == user.Id && t.UsedAt == null)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => t.Id)
            .FirstOrDefaultAsync();

        return id == Guid.Empty ? null : id.ToString();
    }

    public static FakeEmailsService GetFakeEmailsService(this BackFactory factory)
    {
        return (FakeEmailsService)factory.Services.GetRequiredService<IEmailsService>();
    }

    public static SykiDbContext GetDbContext(this BackFactory factory)
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<SykiDbContext>();
    }

    public static async Task SetPassword(this BackFactory factory, string email, string password)
    {
        var scope = factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SykiUser>>();
        var user = await userManager.FindByEmailAsync(email);
        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user!);
        await userManager.ResetPasswordAsync(user!, resetToken, password);
    }

    public static async Task<TestsHttpClient> LoggedAsDirector(this BackFactory factory, string? email = null)
    {
        await using var ctx = factory.GetDbContext();
        var client = factory.GetTestsClient();

        email ??= DataGen.Email;
        var user = (await client.RegisterUser(email)).Success;

        var token = await factory.GetMagicLink(email);
        await client.MagicLinkLogin(token!);

        await factory.SetPassword(email, "My@nEw@strong@P4ssword");

        client.User = user;

        return client;
    }

    public static async Task<TestsHttpClient> LoggedAsTeacher(this BackFactory factory)
    {
        var directorClient = await factory.LoggedAsDirector();

        var email = DataGen.Email;
        var result = await directorClient.CreateTeacher(DataGen.UserName, email);

        await using var ctx = factory.GetDbContext();
        var user = await ctx.Users.Where(u => u.Email == email).FirstAsync();
        var magicLink = new MagicLink(user);
        await ctx.SaveChangesAsync(magicLink);

        var client = factory.GetTestsClient();

        var token = await factory.GetMagicLink(email);
        await client.MagicLinkLogin(token!);

        client.User = new TestsUserDto  { Id = user.Id, InstitutionId = user.InstitutionId, Email = user.Email! };

        return client;
    }

    public static async Task<TestsHttpClient> LoggedAs(this BackFactory factory, string roleName, List<SykiPermission> permissions, UserType baseType = UserType.Manager)
    {
        await using var ctx = factory.GetDbContext();
        var client = factory.GetTestsClient();

        var email = DataGen.Email;
        var user = (await client.RegisterUser(email)).Success;

        // Create custom role with specified permissions (bypassing validation)
        var permissionIds = permissions.Select(p => p.Id).ToList();
        var role = new SykiRole(user.InstitutionId, roleName, roleName, baseType, permissionIds);
        var userRole = new SykiUserRole(user.InstitutionId, user.Id, role.Id) { Role = role };

        ctx.AddRange(role, userRole);
        await ctx.SaveChangesAsync();

        var token = await factory.GetMagicLink(email);
        await client.MagicLinkLogin(token!);

        client.User = user;

        return client;
    }

    public static async Task<string?> GetResetPasswordToken(this BackFactory factory, string email)
    {
        await using var ctx = factory.GetDbContext();

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
