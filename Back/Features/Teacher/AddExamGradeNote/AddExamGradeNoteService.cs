namespace Syki.Back.Features.Teacher.AddExamGradeNote;

public class AddExamGradeNoteService(SykiDbContext ctx) : ITeacherService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Add(Guid teacherId, Guid examGradeId, AddExamGradeNoteIn data)
    {
        var examGrade = await ctx.ExamGrades.Where(x => x.Id == examGradeId).FirstAsync();

        var classOk = await ctx.Classes.AnyAsync(x => x.Id == examGrade.ClassId && x.TeacherId == teacherId);
        if (!classOk) return new TeacherIsNotTheClassLeader();

        examGrade.AddNote(data.Note);
        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
