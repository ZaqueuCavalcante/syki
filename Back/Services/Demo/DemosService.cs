using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Configs;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Microsoft.EntityFrameworkCore;
using Syki.Back.Tasks;

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

    public async Task<DemoOut> Create(DemoIn data)
    {
        if (!data.Email.IsValidEmail())
            Throw.DE0013.Now();

        var demoExists = await _ctx.Demos.AnyAsync(d => d.Email == data.Email.ToLower());
        if (demoExists)
            Throw.DE1102.Now();

        var demo = new Demo(data.Name, data.Email);
        _ctx.Add(demo);

        var task = new SykiTask(new SendDemoEmailConfirmation { Email = demo.Email });
        _ctx.Add(task);

        await _ctx.SaveChangesAsync();

        return demo.ToOut();
    }

    public async Task<DemoSetupOut> Setup(DemoSetupIn data)
    {
        using var transaction = _ctx.Database.BeginTransaction();

        _ = Guid.TryParse(data.Token, out var id);
        var demo = await _ctx.Demos.FirstOrDefaultAsync(d => d.Id == id);
        if (demo == null)
            Throw.DE1103.Now();

        var faculdade = await _faculdadesService.Create(new FaculdadeIn { Nome = demo.Name, SeedData = true });

        var userIn = new UserIn
        {
            Faculdade = faculdade.Id,
            Name = demo.Name,
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
