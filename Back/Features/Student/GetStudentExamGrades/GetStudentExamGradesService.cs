namespace Syki.Back.Features.Student.GetStudentExamGrades;

public class GetStudentExamGradesService(SykiDbContext ctx)
{
    public async Task<List<DisciplineOut>> Get(Guid userId, Guid courseCurriculumId)
    {
        var courseCurriculum = await ctx.CourseCurriculums.AsNoTracking()
            .Include(g => g.Course)
            .Include(g => g.Disciplines)
            .Include(g => g.Links)
            .FirstAsync(g => g.Id == courseCurriculumId);

        var response = courseCurriculum.ToOut().Disciplines.OrderBy(d => d.Period).ToList();

        var studentClassesStatus = await ctx.ClassesStudents.AsNoTracking()
            .Where(x => x.SykiStudentId == userId)
            .ToListAsync();

        var ids = courseCurriculum.Disciplines.ConvertAll(d => d.Id);
        var classes = await ctx.Classes.Where(t => ids.Contains(t.DisciplineId))
            .Select(x => new { x.Id, x.DisciplineId }).ToListAsync();

        response.ForEach(r =>
        {
            var classId = classes.FirstOrDefault(x => x.DisciplineId == r.Id)?.Id;
            r.StudentDisciplineStatus = studentClassesStatus.FirstOrDefault(x => x.ClassId == classId)?.StudentDisciplineStatus ?? StudentDisciplineStatus.Pendente;
        });

        return response.OrderBy(x => x.Name).ToList();
    }
}
