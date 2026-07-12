namespace Estud.Back.Features.Calendar.CreateCalendarDay;

public class CreateCalendarDayIn : IApiDto<CreateCalendarDayIn>
{
    /// <summary>
    /// Data do dia
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Tipo do dia
    /// </summary>
    public DayType? DayType { get; set; }

    /// <summary>
    /// Descrição do dia. Ex: "Semana de provas".
    /// </summary>
    public string? Description { get; set; }

    public static IEnumerable<(string, CreateCalendarDayIn)> GetExamples() =>
    [
        ("Férias",
        new CreateCalendarDayIn
        {
            Date = new DateTime(2026, 1, 5),
            DayType = Domain.Enums.DayType.Vacation,
            Description = "Férias de verão",
        }),

        ("Feriado regional",
        new CreateCalendarDayIn
        {
            Date = new DateTime(2026, 6, 24),
            DayType = Domain.Enums.DayType.Holiday,
            Description = "São João",
        }),
    ];
}
