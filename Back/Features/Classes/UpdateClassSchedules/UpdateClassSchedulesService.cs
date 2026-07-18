using Estud.Back.Domain.Classes;

namespace Estud.Back.Features.Classes.UpdateClassSchedules;

public class UpdateClassSchedulesService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Update(int classId, UpdateClassSchedulesIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var @class = await ctx.Classes
            .Include(c => c.Teachers)
            .Include(c => c.Schedules)
            .FirstOrDefaultAsync(c => c.Id == classId && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        if (@class.Status is ClassStatus.Started or ClassStatus.Finalized) return ClassAlreadyStarted.I;

        var schedulesResult = data.Schedules
            .ConvertAll(x => (x.Day, x.Start, x.End))
            .ToSchedules();
        if (schedulesResult.IsError) return schedulesResult.Error;
        var newSchedules = schedulesResult.Success;

        var teacherIds = @class.Teachers.Select(t => t.Id).ToList();
        if (teacherIds.Count != 0 && newSchedules.Count != 0)
        {
            var otherSchedules = await ctx.Schedules.AsNoTracking()
                .Where(s => s.ClassId != null && s.ClassId != classId
                    && s.Class!.InstitutionId == institutionId
                    && s.Class.Status != ClassStatus.Finalized
                    && s.Class.Teachers.Any(t => teacherIds.Contains(t.Id)))
                .ToListAsync();

            var hasConflict = newSchedules.Any(ns => otherSchedules.Any(ns.Conflict));
            if (hasConflict) return TeacherScheduleConflict.I;
        }

        ctx.Schedules.RemoveRange(@class.Schedules);
        @class.Schedules = newSchedules;
        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
