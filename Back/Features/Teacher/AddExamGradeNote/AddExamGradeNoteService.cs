namespace Syki.Back.Features.Teacher.AddExamGradeNote;

public class AddExamGradeNoteService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Add(Guid teacherId, Guid examGradeId, AddExamGradeNoteIn data)
    {
        var examGrade = await ctx.ExamGrades.Where(x => x.Id == examGradeId).FirstOrDefaultAsync();
        if (examGrade == null) return new ExamGradeNotFound();

        var classOk = await ctx.Classes.AnyAsync(x => x.Id == examGrade.ClassId && x.TeacherId == teacherId);
        if (!classOk) return new TeacherIsNotTheClassLeader();

        var result = examGrade.AddNote(data.Note);
        if (result.IsError()) return result.GetError();

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
