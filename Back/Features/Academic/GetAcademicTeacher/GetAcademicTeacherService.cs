namespace Syki.Back.Features.Academic.GetAcademicTeacher;

public class GetAcademicTeacherService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<GetAcademicTeacherOut, SykiError>> Get(Guid institutionId, Guid teacherId)
    {
        var teacher = await ctx.Teachers
            .Include(x => x.Disciplines)
            .FirstOrDefaultAsync(p => p.InstitutionId == institutionId && p.Id == teacherId);

        if (teacher == null) return new TeacherNotFound();

        return teacher.ToGetAcademicTeacherOut();
    }
}
