using Syki.Shared;
using Syki.Back.Tasks;
using Syki.Back.Domain;
using Syki.Back.Services;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Syki.Shared.SetupDemo;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.SetupDemo;

public class SetupDemoService
{
    private readonly SykiDbContext _ctx;
    private readonly IAuthService _authService;
    public SetupDemoService(
        SykiDbContext ctx,
        IAuthService authService
    ) {
        _ctx = ctx;
        _authService = authService;
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

        var userIn = UserIn.NewDemoAcademico(institution.Id, demo.Email, data.Password);
        var user = await _authService.RegisterUser(userIn);

        await _ctx.SaveChangesAsync();
        transaction.Commit();
    }
}
