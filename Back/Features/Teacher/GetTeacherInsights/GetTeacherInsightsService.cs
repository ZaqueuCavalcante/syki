namespace Syki.Back.Features.Teacher.GetTeacherInsights;

public class GetTeacherInsightsService(SykiDbContext ctx)
{
    public async Task<TeacherInsightsOut> Get(Guid institutionId, Guid userId)
    {
        var classes = await ctx.Classes
            .Where(x => x.InstitutionId == institutionId && x.TeacherId == userId)
            .CountAsync();
        
        return new() { Classes = classes, Students = 3, Aulas = 89, Average = 7.89M };
    }
}
