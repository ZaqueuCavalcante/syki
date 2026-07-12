namespace Estud.Back.Features.Calendar.UpdateCalendarDay;

public class UpdateCalendarDayOut : IApiDto<UpdateCalendarDayOut>
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public DayType DayType { get; set; }

    public string? Description { get; set; }

    public static IEnumerable<(string, UpdateCalendarDayOut)> GetExamples() =>
    [
        ("Recesso",
        new UpdateCalendarDayOut
        {
            Id = 1,
            Date = new DateTime(2026, 12, 26),
            DayType = Domain.Enums.DayType.Recess,
            Description = "Recesso de fim de ano",
        }),
    ];
}
