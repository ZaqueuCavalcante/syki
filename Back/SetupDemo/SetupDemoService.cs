using Syki.Back.Tasks;
using Syki.Back.Domain;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Syki.Back.CreateUser;
using Syki.Shared.SetupDemo;
using Syki.Shared.CreateUser;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.SetupDemo;

public class SetupDemoService(SykiDbContext ctx, CreateUserService service)
{
    public async Task Setup(SetupDemoIn data)
    {
        using var transaction = ctx.Database.BeginTransaction();

        _ = Guid.TryParse(data.Token, out var id);
        var demo = await ctx.Demos.FirstOrDefaultAsync(d => d.Id == id);
        if (demo == null)
            Throw.DE024.Now();

        demo.Setup();

        var institution = new Faculdade($"DEMO - {demo.Email}");
        ctx.Add(institution);
        ctx.Add(SykiTask.SeedInstitutionDemoData(institution.Id));
        await ctx.SaveChangesAsync();

        var userIn = CreateUserIn.NewDemoAcademico(institution.Id, demo.Email, data.Password);
        var user = await service.Create(userIn);

        await ctx.SaveChangesAsync();
        transaction.Commit();
    }
}
