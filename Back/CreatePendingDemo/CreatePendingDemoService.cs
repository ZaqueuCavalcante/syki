using Syki.Shared;
using Syki.Back.Tasks;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.CreatePendingDemo;

public class CreatePendingDemoService
{
    private readonly SykiDbContext _ctx;
    public CreatePendingDemoService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<DemoOut> Create(DemoIn data)
    {
        if (!data.Email.IsValidEmail())
            Throw.DE016.Now();

        var email = data.Email.ToLower();
        var demoExists = await _ctx.Demos.AnyAsync(d => d.Email == email);
        if (demoExists)
            Throw.DE017.Now();

        var demo = new Demo(data.Email);
        _ctx.Add(demo);

        var task = new SykiTask(new SendDemoEmailConfirmation { Email = demo.Email });
        _ctx.Add(task);

        await _ctx.SaveChangesAsync();

        return demo.ToOut();
    }
}
