namespace Syki.Back.Features.Student.GetStudentExamGrades;

public class GetStudentExamGradesService(SykiDbContext ctx)
{
    public async Task<List<StudentExamGradeOut>> Get(Guid userId, Guid courseCurriculumId)
    {
        var classesStudents = await ctx.ClassesStudents.AsNoTracking()
            .Where(x => x.SykiStudentId == userId).ToListAsync();

        var ids = classesStudents.ConvertAll(x => x.ClassId);
        var classes = await ctx.Classes.AsNoTracking()
            .Include(x => x.Discipline)
            .Select(x => new { ClassId = x.Id, x.DisciplineId, Discipline = x.Discipline.Name })
            .Where(x => ids.Contains(x.ClassId))
            .ToListAsync();

        ids = classes.ConvertAll(x => x.DisciplineId);
        var periods = await ctx.CourseCurriculumDisciplines.AsNoTracking()
            .Select(x => new { x.DisciplineId, x.Period })
            .Where(x => ids.Contains(x.DisciplineId))
            .ToListAsync();

        var examGrades = await ctx.ExamGrades.AsNoTracking()
            .Where(x => x.StudentId == userId).ToListAsync();

        var result = classes.ConvertAll(x =>
        {
            return new StudentExamGradeOut
            {
                Discipline = x.Discipline,
                Period = periods.First(p => p.DisciplineId == x.DisciplineId).Period,
                ExamGrades = examGrades.Where(g => g.ClassId == x.ClassId).Select(x => x.ToOut()).ToList(),
                StudentDisciplineStatus = classesStudents.First(s => s.ClassId == x.ClassId).StudentDisciplineStatus,
            };
        });

        return result.OrderBy(x => x.Period).ThenBy(x => x.Discipline).ToList();
    }
}
