namespace Estud.Back.Features.Teacher.AddStudentClassActivityNote;

public class AddStudentClassActivityNoteService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Add(Guid teacherId, Guid classActivityId, AddStudentClassActivityNoteIn data)
    {
        var activity = await ctx.ClassActivities.Where(x => x.Id == classActivityId).FirstOrDefaultAsync();
        if (activity == null) return new ClassActivityNotFound();

        var classOk = await ctx.Classes.AnyAsync(x => x.Id == activity.ClassId && x.TeacherId == teacherId);
        if (!classOk) return new ClassNotFound();

        var work = await ctx.ClassActivityWorks
            .Where(x => x.ClassActivityId == classActivityId && x.EstudStudentId == data.StudentId)
            .FirstOrDefaultAsync();
        if (work == null) return new ClassActivityNotFound();

        var result = work.AddNote(data.Value);
        if (result.IsError) return result.Error;

        await ctx.SaveChangesAsync();

        return new EstudSuccess();
    }
}
