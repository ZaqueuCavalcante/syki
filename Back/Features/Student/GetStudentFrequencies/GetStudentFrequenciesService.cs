namespace Syki.Back.Features.Student.GetStudentFrequencies;

public class GetStudentFrequenciesService(SykiDbContext ctx) : IStudentService
{
    public async Task<OneOf<List<GetStudentFrequenciesOut>, ErrorOut>> Get(Guid userId, Guid courseCurriculumId)
    {
        var attendances = await ctx.Attendances.AsNoTracking().Where(x => x.StudentId == userId).ToListAsync();
        var totalPresences = attendances.Count(x => x.Present);
        
        var result = new List<GetStudentFrequenciesOut>();
        result.Add(new("Total do curso", "-", attendances.Count, totalPresences));
        
        var classIds = attendances.Select(x => x.ClassId).Distinct();
        var classes = await ctx.Classes.AsNoTracking()
            .Include(g => g.Discipline)
            .Where(x => classIds.Contains(x.Id))
            .ToListAsync();

        var disciplineIds = classes.Select(x => x.DisciplineId);
        var disciplines = await ctx.CourseCurriculumDisciplines.AsNoTracking()
            .Where(x => x.CourseCurriculumId == courseCurriculumId && disciplineIds.Contains(x.DisciplineId))
            .ToListAsync();

        var temp = new List<GetStudentFrequenciesOut>();
        foreach (var item in classes)
        {
            var period = disciplines.First(x => x.DisciplineId == item.DisciplineId).Period.ToString();
            var lessons = attendances.Count(x => x.ClassId == item.Id);
            var presences = attendances.Count(x => x.ClassId == item.Id && x.Present);
            temp.Add(new(item.Discipline.Name, period, lessons, presences));
        }
        
        result.AddRange(temp.OrderBy(x => x.Period).ThenBy(x => x.Name));

        return result;
    }
}
