namespace Syki.Back.Features.Student.GetStudentAverageNote;

public class GetStudentAverageNoteService(SykiDbContext ctx) : IStudentService
{
    public async Task<decimal> Get(Guid userId)
    {
        var classesStudents = await ctx.ClassesStudents.AsNoTracking()
            .Where(x =>
                x.SykiStudentId == userId &&
                x.StudentDisciplineStatus != StudentDisciplineStatus.Pendente &&
                x.StudentDisciplineStatus != StudentDisciplineStatus.Matriculado
            ).ToListAsync();

        var classes = classesStudents.ConvertAll(x => x.ClassId);
        var examGrades = await ctx.ExamGrades.AsNoTracking()
            .Where(x => x.StudentId == userId).ToListAsync();

        var notes = classes.ConvertAll(c => examGrades.Where(g => g.ClassId == c).GetAverageNote());

        if (notes.Count == 0) return 0;

        return Math.Round(notes.Average(), 2);
    }
}
