namespace Syki.Back.CreateEvaluationUnits;

public class CreateEvaluationUnitsService(SykiDbContext ctx)
{
    public async Task Create(Guid institutionId, CreateEvaluationUnitsIn data)
    {
        var turmaOk = await ctx.Turmas
            .AnyAsync(o => o.FaculdadeId == institutionId && o.Id == data.TurmaId);
        if (!turmaOk)
            Throw.DE029.Now();



    }
}
