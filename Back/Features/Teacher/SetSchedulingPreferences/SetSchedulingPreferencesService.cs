using Syki.Back.Features.Academic.CreateClass;

namespace Syki.Back.Features.Teacher.SetSchedulingPreferences;

public class SetSchedulingPreferencesService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Set(Guid institutionId, Guid teacherId, SetSchedulingPreferencesIn data)
    {
        var teacher = await ctx.Teachers.Include(x => x.SchedulingPreferences).FirstAsync(p => p.InstitutionId == institutionId && p.Id == teacherId);

        var schedules = data.Schedules.ConvertAll(h => Schedule.New(h.Day, h.Start, h.End));
        foreach (var schedule in schedules)
        {
            if (schedule.IsError) return schedule.Error;
        }

        teacher.SchedulingPreferences = schedules.ConvertAll(x => x.Success);

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
