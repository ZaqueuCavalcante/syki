using Microsoft.AspNetCore.Identity;
using Syki.Back.Features.Cross.CreateUser;
using Microsoft.Extensions.DependencyInjection;
using Syki.Back.Features.Cross.CreateInstitution;

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

    public static async Task<HttpClient> LoggedAsAcademic(this BackWebApplicationFactory factory)
    {
        var client = factory.GetClient();
        var user = await client.RegisterUser(factory);
        await client.Login(user.Email, user.Password);
        return client;
    }

    public static async Task RegisterAdm(this BackWebApplicationFactory factory)
    {
        using var ctx = factory.GetDbContext();
        using var userManager = factory.GetUserManager();

        var institution = new Institution { Id = Guid.Empty, Name = "Syki" };
        ctx.Add(institution);
        await ctx.SaveChangesAsync();

        var userIn = new CreateUserIn
        {
            Name = "Adm",
            Email = "adm@syki.com",
            Role = UserRole.Adm,
            Password = "Test@123",
            InstitutionId = institution.Id,
        };

        var user = new SykiUser(institution.Id, userIn.Name, userIn.Email);
        await userManager.CreateAsync(user, userIn.Password);

        await userManager.AddToRoleAsync(user, userIn.Role.ToString());
    }

    public static async Task<HttpClient> LoggedAsAdm(this BackWebApplicationFactory factory)
    {
        var client = factory.GetClient();
        await client.Login("adm@syki.com", "Test@123");
        return client;
    }

    public static async Task<HttpClient> LoggedAsStudent(this BackWebApplicationFactory factory, string email)
    {
        var client = factory.GetClient();

        var token = await factory.GetResetPasswordToken(email);
        var password = await client.ResetPassword(token!);
        await client.Login(email, password);
    
        return client;
    }

    public static async Task<HttpClient> LoggedAsTeacher(this BackWebApplicationFactory factory, string email)
    {
        var client = factory.GetClient();

        var token = await factory.GetResetPasswordToken(email);
        var password = await client.ResetPassword(token!);
        await client.Login(email, password);
    
        return client;
    }

    public static SykiDbContext GetDbContext(this BackWebApplicationFactory factory)
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<SykiDbContext>();
    }

    public static UserManager<SykiUser> GetUserManager(this BackWebApplicationFactory factory)
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<UserManager<SykiUser>>();
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

    public static async Task AwaitTasksProcessing(this BackWebApplicationFactory factory)
    {
        using var ctx = factory.GetDbContext();
        while (true)
        {
            var tasks = await ctx.Tasks.CountAsync(x => x.ProcessedAt == null);
            if (tasks == 0) break;
        }
    }
}
