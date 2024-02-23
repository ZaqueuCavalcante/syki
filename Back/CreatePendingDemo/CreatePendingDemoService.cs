using Syki.Back.Tasks;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Syki.Shared.CreatePendingDemo;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.CreatePendingDemo;

public class CreatePendingDemoService(SykiDbContext ctx)
{
    public async Task Create(CreatePendingDemoIn data)
    {
        var email = data.Email.ToLower();
        var demoExists = await ctx.Demos.AnyAsync(d => d.Email == email);
        if (demoExists)
            Throw.DE017.Now();

        var demo = new Demo(data.Email);
        ctx.Add(demo);

        var task = new SykiTask(new SendDemoEmailConfirmation { Email = demo.Email });
        ctx.Add(task);

        await ctx.SaveChangesAsync();
    }
}
