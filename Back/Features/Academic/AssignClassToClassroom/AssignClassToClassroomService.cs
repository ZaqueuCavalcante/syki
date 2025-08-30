using Syki.Back.Features.Academic.CreateClass;

namespace Syki.Back.Features.Academic.AssignClassToClassroom;

public class AssignClassToClassroomService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Assign(Guid institutionId, Guid classroomId, AssignClassToClassroomIn data)
    {
        var classroom = await ctx.Classrooms.FirstOrDefaultAsync(c => c.InstitutionId == institutionId && c.Id == classroomId);
        if (classroom == null) return new ClassroomNotFound();

        var @class = await ctx.Classes.FirstOrDefaultAsync(c => c.InstitutionId == institutionId && c.Id == data.ClassId);
        if (@class == null) return new ClassNotFound();

        if (@class.CampusId != classroom.CampusId) return new ClassAndClassroomShouldStayOnSameCampus();

        if (@class.Vacancies > classroom.Capacity) return new ClassVacanciesGreaterThanClassroomCapacity();

        if (data.Schedules == null || data.Schedules.Count == 0) return new InvalidScheduleForAssignClassToClassroom();

        var ids = await ctx.ClassroomsClasses.Where(c => c.ClassroomId == classroomId && c.IsActive).Select(x => x.ClassId).ToListAsync() ?? [];
        var currentSchedules = await ctx.Schedules.Where(x => x.ClassroomId == classroomId && x.ClassId != null && ids.Contains(x.ClassId.Value)).ToListAsync() ?? [];

        data.Schedules.AddRange(currentSchedules.ConvertAll(x => new ScheduleIn(x.Day, x.Start, x.End)));

        var schedulesResult = data.Schedules.ToHashSet().ToList().ToSchedules();
        if (schedulesResult.IsError) return schedulesResult.Error;

        ctx.RemoveRange(currentSchedules);

        var newSchedules = schedulesResult.Success;
        newSchedules.ForEach(x =>
        {
            x.ClassId = @class.Id;
            x.ClassroomId = classroom.Id;
            x.TeacherId = @class.TeacherId;
        });
        ctx.AddRange(newSchedules);

        var assignExists = await ctx.ClassroomsClasses.AnyAsync(x => x.ClassroomId == classroom.Id && x.ClassId == @class.Id);
        if (!assignExists)
        {
            ctx.Add(new ClassroomClass(classroom.Id, @class.Id));
        }

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
