namespace Estud.Back.Domain.Classes;

public static class ScheduleExtensions
{
    public static OneOf<List<Schedule>, EstudError> ToSchedules(this List<(Day Day, Hour Start, Hour End)> items)
    {
        var result = items.ConvertAll(x => Schedule.New(x.Day, x.Start, x.End));
        foreach (var r in result)
            if (r.IsError) return r.Error;

        var schedules = result.ConvertAll(x => x.Success);

        for (int i = 0; i < schedules.Count - 1; i++)
            for (int j = i + 1; j < schedules.Count; j++)
                if (schedules[i].Conflict(schedules[j]))
                    return ConflictingSchedules.I;

        return schedules;
    }
}
