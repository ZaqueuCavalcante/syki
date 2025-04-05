namespace Syki.Back.Features.Teacher.GetTeacherClassNotes;

public class GetTeacherClassNotesService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<List<TeacherClassStudentOut>, SykiError>> Get(Guid teacherId, Guid classId)
    {
        var @class = await ctx.Classes.AsNoTracking()
            .Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.Id == classId && x.TeacherId == teacherId);
        if (@class == null) return new ClassNotFound();

        return new List<TeacherClassStudentOut>();
    }
}
