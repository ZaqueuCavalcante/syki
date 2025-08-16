namespace Syki.Back.Features.Academic.AssignClassToClassroom;

public class AssignClassToClassroomService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Assign(Guid institutionId, Guid classroomId, AssignClassToClassroomIn data)
    {
        var classroom = await ctx.Classrooms.FirstOrDefaultAsync(c => c.InstitutionId == institutionId && c.Id == classroomId);
        if (classroom == null) return new ClassroomNotFound();

        var @class = await ctx.Classes.FirstOrDefaultAsync(c => c.InstitutionId == institutionId && c.Id == data.ClassId);
        if (@class == null) return new ClassNotFound();

        var assign = new ClassroomClass(classroom.Id, @class.Id);
        await ctx.SaveChangesAsync(assign);

        return new SykiSuccess();
    }
}
