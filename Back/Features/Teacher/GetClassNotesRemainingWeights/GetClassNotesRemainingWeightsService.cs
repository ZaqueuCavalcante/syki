namespace Syki.Back.Features.Teacher.GetClassNotesRemainingWeights;

public class GetClassNotesRemainingWeightsService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<List<ClassNoteRemainingWeightsOut>, SykiError>> Get(Guid institutionId, Guid userId, Guid id)
    {
        var @class = await ctx.Classes.AsNoTracking()
            .Include(t => t.Activities)
            .Where(t => t.InstitutionId == institutionId && t.TeacherId == userId && t.Id == id)
            .FirstOrDefaultAsync();

        if (@class == null) return new ClassNotFound();

        return @class.GetNotesRemainingWeights();
    }
}
