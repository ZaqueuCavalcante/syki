namespace Syki.Shared;

public class SetSchedulingPreferencesIn
{
    public List<ScheduleIn> Schedules { get; set; }

    public static IEnumerable<(string, SetSchedulingPreferencesIn)> GetExamples() =>
    [
        ("Único", new() { Schedules = [new(Day.Monday, Hour.H07_00, Hour.H10_00)]}),
        ("Múltiplos", new() { Schedules = [new(Day.Tuesday, Hour.H09_00, Hour.H12_00), new(Day.Thursday, Hour.H14_00, Hour.H18_00)]}),
    ];
}
