namespace Estud.Back.Features.Classes.GetClasses;

public class GetClassesService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetClassesOut> Get(GetAcademicClassesIn query)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var classes = await ctx.Classes.AsNoTracking()
            .Include(c => c.Discipline)
            .Include(c => c.Teacher)
            .Include(c => c.Period)
            .Include(c => c.Schedules)
            .Where(c => c.InstitutionId == institutionId)
            .ToListAsync();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        foreach (var @class in classes)
        {
            if (@class.Status == ClassStatus.OnEnrollment && @class.Period?.EndAt < today)
                @class.Status = ClassStatus.AwaitingStart;
        }

        var statusFilter = query?.Status;
        var items = classes
            .Where(c => statusFilter == null || c.Status == statusFilter)
            .Select(c => new GetClassesItemOut
            {
                Id = c.Id,
                Discipline = c.Discipline?.Name ?? "",
                Teacher = c.Teacher?.Name ?? "",
                Period = c.Period?.Name ?? "",
                Vacancies = c.Vacancies,
                Status = c.Status,
                Schedules = c.Schedules
                    .Select(s => new ScheduleOut(s.Day, s.Start, s.End))
                    .ToList(),
            })
            .ToList();

        return new GetClassesOut { Total = items.Count, Items = items };
    }
}
