using Syki.Shared;
using Syki.Back.Tasks;
using Syki.Back.Domain;
using Syki.Back.Configs;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class DemosService : IDemosService
{
    private readonly SykiDbContext _ctx;
    private readonly IAuthService _authService;
    private readonly IFaculdadesService _faculdadesService;
    public DemosService(
        SykiDbContext ctx,
        IAuthService authService,
        IFaculdadesService faculdadesService
    ) {
        _ctx = ctx;
        _authService = authService;
        _faculdadesService = faculdadesService;
    }

    public async Task<DemoSetupOut> Setup(DemoSetupIn data)
    {
        using var transaction = _ctx.Database.BeginTransaction();

        _ = Guid.TryParse(data.Token, out var id);
        var demo = await _ctx.Demos.FirstOrDefaultAsync(d => d.Id == id);
        if (demo == null)
            Throw.DE024.Now();

        if (demo.Start != null)
            Throw.DE025.Now();

        var faculdade = await _faculdadesService.Create(new FaculdadeIn { Nome = "DEMO", SeedData = true });

        var userIn = new UserIn
        {
            Faculdade = faculdade.Id,
            Name = "DEMO",
            Email = demo.Email,
            Password = data.Password,
            Role = AuthorizationConfigs.Academico,
        };
        var user = await _authService.Register(userIn);

        demo.Setup();
        await _ctx.SaveChangesAsync();

        transaction.Commit();

        return new DemoSetupOut { Ok = true };
    }
}
