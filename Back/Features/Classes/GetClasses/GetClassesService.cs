namespace Estud.Back.Features.Classes.GetClasses;

public class GetClassesService(EstudDbContext ctx) : IEstudService
{
    private const int MaxPageSize = 100;

    public async Task<GetClassesOut> Get(GetClassesIn query)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);

        var institutionId = ctx.RequestUser.InstitutionId;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var hasCurrentEnrollmentPeriod = await ctx.EnrollmentPeriods.AsNoTracking()
            .AnyAsync(p => p.InstitutionId == institutionId && p.StartAt <= today && today <= p.EndAt);

        var classesQuery = ctx.Classes.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId);

        // Sem período de matrícula aberto, turmas OnEnrollment aparecem como OnReview,
        // então o filtro precisa considerar o status exibido, e não o salvo no banco.
        if (query.Status is ClassStatus status)
        {
            if (!hasCurrentEnrollmentPeriod && status == ClassStatus.OnEnrollment)
                return new GetClassesOut { Total = 0, Page = page, PageSize = pageSize };

            if (!hasCurrentEnrollmentPeriod && status == ClassStatus.OnReview)
                classesQuery = classesQuery.Where(c =>
                    c.Status == ClassStatus.OnReview || c.Status == ClassStatus.OnEnrollment);
            else
                classesQuery = classesQuery.Where(c => c.Status == status);
        }

        var total = await classesQuery.CountAsync();

        var classes = await classesQuery
            .Include(c => c.Discipline)
            .Include(c => c.Teachers)
            .Include(c => c.Period)
            .Include(c => c.Schedules)
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (!hasCurrentEnrollmentPeriod)
        {
            foreach (var @class in classes)
            {
                if (@class.Status == ClassStatus.OnEnrollment)
                    @class.Status = ClassStatus.OnReview;
            }
        }

        var items = classes.ConvertAll(c => new GetClassesItemOut
        {
            Id = c.Id,
            Discipline = c.Discipline?.Name ?? "",
            Teachers = c.Teachers.Select(t => t.Name).Order().ToList(),
            Period = c.Period?.Name ?? "",
            Vacancies = c.Vacancies,
            Status = c.Status,
            Schedules = c.Schedules
                .Select(s => new GetClassesScheduleOut(s.Day, s.Start, s.End))
                .ToList(),
        });

        return new GetClassesOut
        {
            Total = total,
            Page = page,
            PageSize = pageSize,
            Items = items,
        };
    }
}
