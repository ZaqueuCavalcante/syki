namespace Estud.Back.Features.Calendar.CreateCalendarDay;

public class CreateCalendarDayOut : IApiDto<CreateCalendarDayOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateCalendarDayOut)> GetExamples() =>
    [
        ("Férias", new CreateCalendarDayOut { Id = 1 })
    ];
}
