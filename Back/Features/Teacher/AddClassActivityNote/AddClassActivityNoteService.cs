namespace Syki.Back.Features.Teacher.AddClassActivityNote;

public class AddClassActivityNoteService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Add(Guid teacherId, Guid noteId, AddClassActivityNoteIn data)
    {
        var note = await ctx.Notes.Where(x => x.Id == noteId).FirstOrDefaultAsync();
        if (note == null) return new ClassActivityNoteNotFound();

        var classOk = await ctx.Classes.AnyAsync(x => x.Id == note.ClassId && x.TeacherId == teacherId);
        if (!classOk) return new ClassNotFound();

        var result = note.AddNote(data.Note);
        if (result.IsError()) return result.GetError();

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
