namespace Syki.Back.Features.Student.GetStudentNotes;

public class GetStudentNotesService(SykiDbContext ctx) : IStudentService
{
    public async Task<List<StudentNoteOut>> Get(Guid userId)
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

        var notes = await ctx.Notes.AsNoTracking()
            .Where(x => x.StudentId == userId).ToListAsync();

        var result = classes.ConvertAll(c =>
        {
            var disciplineNotes = notes.Where(g => g.ClassId == c.ClassId).ToList();
            return new StudentNoteOut
            {
                ClassId = c.ClassId,
                Discipline = c.Discipline,
                AverageNote = disciplineNotes.GetAverageNote(),
                Notes = disciplineNotes.Select(g => g.ToOut()).ToList(),
                Period = periods.First(p => p.DisciplineId == c.DisciplineId).Period,
                StudentDisciplineStatus = classesStudents.First(s => s.ClassId == c.ClassId).StudentDisciplineStatus,
            };
        });

        return result.OrderBy(x => x.Period).ThenBy(x => x.Discipline).ToList();
    }
}
