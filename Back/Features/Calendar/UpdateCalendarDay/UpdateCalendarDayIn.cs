namespace Estud.Back.Features.Calendar.UpdateCalendarDay;

public class UpdateCalendarDayIn : IApiDto<UpdateCalendarDayIn>
{
    public int Id { get; set; }

    /// <summary>
    /// Tipo do dia
    /// </summary>
    public DayType? DayType { get; set; }

    /// <summary>
    /// Descrição do dia. Ex: "Semana de provas".
    /// </summary>
    public string? Description { get; set; }

    public static IEnumerable<(string, UpdateCalendarDayIn)> GetExamples() =>
    [
        ("Recesso",
        new UpdateCalendarDayIn
        {
            Id = 1,
            DayType = Domain.Enums.DayType.Recess,
            Description = "Recesso de fim de ano",
        }),
    ];
}
