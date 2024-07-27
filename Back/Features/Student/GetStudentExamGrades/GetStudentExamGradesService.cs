namespace Syki.Back.Features.Student.GetStudentExamGrades;

public class GetStudentExamGradesService(SykiDbContext ctx)
{
    public async Task<List<StudentExamGradeOut>> Get(Guid userId)
    {
        var classesStudents = await ctx.ClassesStudents.AsNoTracking()
            .Where(x => x.SykiStudentId == userId).ToListAsync();

        var classesIds = classesStudents.ConvertAll(x => x.ClassId);
        var classes = await ctx.Classes.AsNoTracking()
            .Include(x => x.Discipline)
            .Select(x => new { ClassId = x.Id, x.DisciplineId, Discipline = x.Discipline.Name })
            .Where(x => classesIds.Contains(x.ClassId))
            .ToListAsync();

        var disciplinesIds = classes.ConvertAll(x => x.DisciplineId);
        var periods = await ctx.CourseCurriculumDisciplines.AsNoTracking()
            .Select(x => new { x.DisciplineId, x.Period })
            .Where(x => disciplinesIds.Contains(x.DisciplineId))
            .ToListAsync();

        var examGrades = await ctx.ExamGrades.AsNoTracking()
            .Where(x => x.StudentId == userId).ToListAsync();

        var result = classes.ConvertAll(c =>
        {
            var disciplineExamGrades = examGrades.Where(g => g.ClassId == c.ClassId).ToList();
            return new StudentExamGradeOut
            {
                Discipline = c.Discipline,
                Period = periods.First(p => p.DisciplineId == c.DisciplineId).Period,
                AverageNote = disciplineExamGrades.GetAverageNote(),
                ExamGrades = disciplineExamGrades.Select(g => g.ToOut()).ToList(),
                StudentDisciplineStatus = classesStudents.First(s => s.ClassId == c.ClassId).StudentDisciplineStatus,
            };
        });

        return result.OrderBy(x => x.Period).ThenBy(x => x.Discipline).ToList();
    }
}
