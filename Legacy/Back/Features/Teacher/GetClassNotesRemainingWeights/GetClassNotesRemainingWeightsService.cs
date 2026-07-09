namespace Estud.Back.Features.Teacher.GetClassNotesRemainingWeights;

public class GetClassNotesRemainingWeightsService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<List<ClassNoteRemainingWeightsOut>, EstudError>> Get(Guid institutionId, Guid userId, Guid id)
    {
        var @class = await ctx.Classes.AsNoTracking()
            .Include(t => t.Activities)
            .Where(t => t.InstitutionId == institutionId && t.TeacherId == userId && t.Id == id)
            .FirstOrDefaultAsync();

        if (@class == null) return new ClassNotFound();

        return @class.GetNotesRemainingWeights();
    }
}
