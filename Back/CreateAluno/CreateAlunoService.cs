using Syki.Back.CreateUser;
using Syki.Back.SendResetPasswordToken;

namespace Syki.Back.CreateAluno;

public class CreateAlunoService(SykiDbContext ctx, CreateUserService service, SendResetPasswordEmailService sendService)
{
    public async Task<AlunoOut> Create(Guid institutionId, AlunoIn data)
    {
        using var transaction = ctx.Database.BeginTransaction();

        var ofertaOk = await ctx.Ofertas
            .AnyAsync(o => o.FaculdadeId == institutionId && o.Id == data.OfertaId);
        if (!ofertaOk)
            Throw.DE012.Now();

        var userIn = CreateUserIn.NewAluno(institutionId, data.Email);
        var user = await service.Create(userIn);

        var aluno = new Aluno(user.Id, institutionId, data.Nome, data.OfertaId);
        ctx.Add(aluno);
        await ctx.SaveChangesAsync();

        await sendService.Send(new SendResetPasswordTokenIn { Email = user.Email });

        transaction.Commit();

        return aluno.ToOut();
    }
}
