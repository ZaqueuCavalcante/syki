using Estud.Back.Domain.Calendar;

namespace Estud.Back.Features.Students.GetStudentAttendanceCalendar;

public class GetStudentAttendanceCalendarService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetStudentAttendanceCalendarOut> Get(GetStudentAttendanceCalendarIn data)
    {
        var year = data.Year ?? DateTime.UtcNow.Year;

        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var studentId = await ctx.GetStudentId(institutionId, userId);

        var start = new DateOnly(year, 1, 1);
        var end = new DateOnly(year, 12, 31);

        var customDays = await ctx.CalendarDays.AsNoTracking()
            .Where(d => d.InstitutionId == institutionId)
            .Where(d => d.Date >= start && d.Date <= end)
            .ToDictionaryAsync(d => d.Date);

        var holidays = NationalHolidays.OfYear(year);

        // Turmas em que o aluno está matriculado
        var classIds = await ctx.ClassStudents.AsNoTracking()
            .Where(cs => cs.StudentId == studentId && cs.Status == StudentClassStatus.Matriculado)
            .Select(cs => cs.ClassId)
            .ToListAsync();

        // Aulas do aluno no ano, com a frequência dele quando já lançada (null = ainda não lançada)
        var lessons = await ctx.ClassLessons.AsNoTracking()
            .Where(l => classIds.Contains(l.ClassId) && l.Date >= start && l.Date <= end)
            .Select(l => new
            {
                l.Date,
                Present = l.Attendances
                    .Where(a => a.StudentId == studentId)
                    .Select(a => (bool?)a.Present)
                    .FirstOrDefault(),
            })
            .ToListAsync();

        var presenceByDate = lessons
            .GroupBy(l => l.Date)
            .ToDictionary(g => g.Key, g => g.Select(l => l.Present).ToList());

        var items = new List<GetStudentAttendanceCalendarItemOut>();
        for (var date = start; date <= end; date = date.AddDays(1))
        {
            items.Add(new GetStudentAttendanceCalendarItemOut
            {
                Date = date.ToDateTime(TimeOnly.MinValue),
                Status = ResolveStatus(date, customDays, holidays, presenceByDate),
            });
        }

        return new GetStudentAttendanceCalendarOut
        {
            Year = year,
            Total = items.Count,
            Items = items,
        };
    }

    private static StudentDayAttendanceStatus ResolveStatus(
        DateOnly date,
        Dictionary<DateOnly, CalendarDay> customDays,
        Dictionary<DateOnly, string> holidays,
        Dictionary<DateOnly, List<bool?>> presenceByDate
    ) {
        var dayType = ResolveDayType(date, customDays, holidays);

        // Dia sem aula para a instituição (fim de semana, feriado, férias, recesso)
        if (dayType != DayType.Default) return StudentDayAttendanceStatus.NoClass;

        // Dia letivo em que o aluno não tem nenhuma aula agendada
        if (!presenceByDate.TryGetValue(date, out var presences)) return StudentDayAttendanceStatus.NoClass;

        // Aula(s) do aluno sem frequência lançada ainda (futura ou pendente)
        var recorded = presences.Where(p => p.HasValue).ToList();
        if (recorded.Count == 0) return StudentDayAttendanceStatus.Undefined;

        // Falta em qualquer aula do dia já lançada conta como falta
        if (recorded.Any(p => p == false)) return StudentDayAttendanceStatus.Absent;

        return StudentDayAttendanceStatus.Present;
    }

    private static DayType ResolveDayType(
        DateOnly date,
        Dictionary<DateOnly, CalendarDay> customDays,
        Dictionary<DateOnly, string> holidays
    ) {
        if (customDays.TryGetValue(date, out var customDay)) return customDay.DayType;

        if (holidays.ContainsKey(date)) return DayType.Holiday;

        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) return DayType.Weekend;

        return DayType.Default;
    }
}
