using Syki.Back.RegisterUser;

namespace Syki.Back.CreateProfessor;

public class CreateProfessorService(SykiDbContext ctx, RegisterUserService service)
{
    public async Task<ProfessorOut> Create(Guid faculdadeId, ProfessorIn data)
    {
        using var transaction = ctx.Database.BeginTransaction();

        var userIn = new CreateUserIn
        {
            InstitutionId = faculdadeId,
            Name = data.Nome,
            Email = data.Email,
            Password = $"Professor@{Guid.NewGuid().ToString().OnlyNumbers()}",
            Role = AuthorizationConfigs.Professor,
        };
        var user = await service.Register(userIn);

        var professor = new Professor(user.Id, faculdadeId, data.Nome);

        ctx.Add(professor);
        await ctx.SaveChangesAsync();

        transaction.Commit();

        return professor.ToOut();
    }
}
