namespace Estud.Back.Features.Teachers.GetTeacherCurrentClasses;

public class GetTeacherCurrentClassesService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetTeacherCurrentClassesOut> Get()
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var teacherId = await ctx.GetTeacherId(institutionId, userId);

        var classes = await ctx.Classes.AsNoTracking()
            .Where(t => t.InstitutionId == institutionId && t.Teachers.Any(x => x.Id == teacherId) && t.Status == ClassStatus.Started)
            .OrderBy(t => t.Discipline.Name)
            .Select(t => new GetTeacherCurrentClassesItemOut
            {
                Id = t.Id,
                Name = t.Discipline.Name,
            })
            .ToListAsync();

        return new GetTeacherCurrentClassesOut { Classes = classes };
    }
}
