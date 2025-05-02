namespace Syki.Back.Features.Academic.FinalizeClasses;

public class FinalizeClassesService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Finalize(Guid institutionId, FinalizeClassesIn data)
    {
        var classes = await ctx.Classes
            .Include(x => x.Lessons)
                .ThenInclude(x => x.Attendances)
            .Include(x => x.Students)
            .Include(t => t.Notes)
            .Where(c => c.InstitutionId == institutionId && data.Classes.Contains(c.Id))
            .ToListAsync();

        if (classes.Count == 0) return new InvalidClassesList();
        
        var statusOk = classes.All(x => x.Status == ClassStatus.Started);
        if (!statusOk) return new ClassMustHaveStartedStatus();

        var results = classes.ConvertAll(c => c.Finish());
        foreach (var result in results)
        {
            if (result.IsError()) return result.GetError();
        }

        var ids = classes.ConvertAll(x => x.Id);
        var classesStudents = await ctx.ClassesStudents.Where(x => ids.Contains(x.ClassId)).ToListAsync();

        var configs = await ctx.Configs.FirstAsync(x => x.InstitutionId == institutionId);

        foreach (var @class in classes)
        {
            var lessons = @class.Lessons.Count(x => x.Attendances.Count > 0);
            foreach (var student in @class.Students)
            {
                // TODO: Calculate notes using the class activities
                var studentNotes = @class.Notes.Where(g => g.StudentId == student.Id).ToList();
                var averageNote = studentNotes.GetAverageNote();
                var presences = @class.Lessons.Count(x => x.Attendances.Exists(a => a.StudentId == student.Id && a.Present));
                var frequency = lessons == 0 ? 0.00M : 100M * (1M * presences / (1M * lessons));
                var link = classesStudents.First(x => x.ClassId == @class.Id && x.SykiStudentId == student.Id);

                link.StudentDisciplineStatus = StudentDisciplineStatus.Aprovado;
                if (averageNote < configs.NoteLimit)
                {
                    link.StudentDisciplineStatus = StudentDisciplineStatus.ReprovadoPorNota;
                }
                else if (frequency < configs.FrequencyLimit)
                {
                    link.StudentDisciplineStatus = StudentDisciplineStatus.ReprovadoPorFalta;
                }
            }
        }

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
