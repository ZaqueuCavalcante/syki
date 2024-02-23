using Syki.Shared;
using Syki.Back.Tasks;
using Syki.Back.Domain;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Syki.Back.CreateUser;
using Syki.Shared.SetupDemo;
using Microsoft.EntityFrameworkCore;
using Syki.Shared.CreateUser;

namespace Syki.Back.SetupDemo;

public class SetupDemoService
{
    private readonly SykiDbContext _ctx;
    private readonly CreateUserService _service;
    public SetupDemoService(
        SykiDbContext ctx,
        CreateUserService service
    ) {
        _ctx = ctx;
        _service = service;
    }

    public async Task Setup(SetupDemoIn data)
    {
        using var transaction = _ctx.Database.BeginTransaction();

        _ = Guid.TryParse(data.Token, out var id);
        var demo = await _ctx.Demos.FirstOrDefaultAsync(d => d.Id == id);
        if (demo == null)
            Throw.DE024.Now();

        demo.Setup();

        var institution = new Faculdade($"DEMO - {demo.Email}");
        _ctx.Add(institution);
        _ctx.Add(SykiTask.SeedInstitutionDemoData(institution.Id));
        await _ctx.SaveChangesAsync();

        var userIn = CreateUserIn.NewDemoAcademico(institution.Id, demo.Email, data.Password);
        var user = await _service.Create(userIn);

        await _ctx.SaveChangesAsync();
        transaction.Commit();
    }
}
