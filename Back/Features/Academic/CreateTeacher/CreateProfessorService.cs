using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Back.CreateProfessor;

public class CreateProfessorService(SykiDbContext ctx, CreateUserService service, SendResetPasswordEmailService sendService)
{
    public async Task<ProfessorOut> Create(Guid institutionId, ProfessorIn data)
    {
        using var transaction = ctx.Database.BeginTransaction();

        var userIn = CreateUserIn.NewTeacher(institutionId, data.Name, data.Email);
        var user = await service.Create(userIn);

        var professor = new Professor(user.Id, institutionId, data.Name);

        ctx.Add(professor);
        await ctx.SaveChangesAsync();

        if (!data.Email.EndsWith("syki.demo.com"))
        {
            await sendService.Send(new SendResetPasswordTokenIn { Email = user.Email });
        }

        transaction.Commit();

        return professor.ToOut();
    }
}
