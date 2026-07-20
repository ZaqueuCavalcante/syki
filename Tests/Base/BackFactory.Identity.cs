using Estud.Back.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Estud.Tests.Integration.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace Estud.Tests.Base;

public static class BackFactoryIdentity
{
    public static async Task<string?> GetMagicLinkToken(this BackFactory factory, string email)
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

    public static async Task SetPassword(this BackFactory factory, string email, string password)
    {
        var scope = factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<EstudUser>>();
        var user = await userManager.FindByEmailAsync(email);
        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user!);
        await userManager.ResetPasswordAsync(user!, resetToken, password);
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

    public static async Task<TestsHttpClient> LoggedAsDirector(this BackFactory factory, string? email = null)
    {
        await using var ctx = factory.GetDbContext();
        var client = factory.GetTestsClient();

        email ??= DataGen.Email;
        var user = await client.RegisterUser(email).Success();

        var token = await factory.GetMagicLinkToken(email);
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

        var token = await factory.GetMagicLinkToken(email);
        await client.MagicLinkLogin(token!);

        var institutionId = await ctx.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.InstitutionId).FirstAsync();
        client.User = new TestsUserDto { Id = user.Id, InstitutionId = institutionId, Email = user.Email! };

        return client;
    }

    public static async Task<TestsHttpClient> LoginAs(this BackFactory factory, string email)
    {
        await using var ctx = factory.GetDbContext();
        var user = await ctx.Users.Where(u => u.Email == email).FirstAsync();
        var magicLink = new MagicLink(user);
        await ctx.SaveChangesAsync(magicLink);

        var client = factory.GetTestsClient();

        var token = await factory.GetMagicLinkToken(email);
        await client.MagicLinkLogin(token!);

        var institutionId = await ctx.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.InstitutionId).FirstAsync();
        client.User = new TestsUserDto { Id = user.Id, InstitutionId = institutionId, Email = user.Email! };

        return client;
    }
}
