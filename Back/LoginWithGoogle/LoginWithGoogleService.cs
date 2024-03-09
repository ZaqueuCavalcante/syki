using Syki.Back.CreateUser;
using Syki.Back.GenerateJWT;
using Syki.Back.CreatePendingUserRegister;

namespace Syki.Back.LoginWithGoogle;

public class LoginWithGoogleService(SykiDbContext ctx, CreateUserService createUserService, GenerateJWTService generateJWTService)
{
    public async Task<LoginOut> Login(string email)
    {
        email = email.ToLower();
        var userExists = await ctx.Users.AnyAsync(u => u.Email == email);
        if (userExists)
        {
            var token = await generateJWTService.Generate(email);
            return new LoginOut { AccessToken = token };
        }

        using var transaction = ctx.Database.BeginTransaction();

        var register = await ctx.UserRegisters.FirstOrDefaultAsync(d => d.Email == email);
        if (register == null)
        {
            register = new UserRegister(email);
            ctx.Add(register);
        }
        register.Finish();

        var institution = new Faculdade($"Instituição - {register.Email}");
        ctx.Add(institution);
        ctx.Add(SykiTask.SeedInstitutionData(institution.Id));
        await ctx.SaveChangesAsync();

        var userIn = CreateUserIn.NewAcademico(institution.Id, register.Email);
        await createUserService.Create(userIn);

        await ctx.SaveChangesAsync();
        transaction.Commit();

        var jwt = await generateJWTService.Generate(email);

        return new LoginOut { AccessToken = jwt };
    }
}
