namespace Syki.Back.Features.Academic.CreateClass;

public static class ScheduleExtensions
{
    extension(List<ScheduleIn> schedulesIn)
    {
        public OneOf<List<Schedule>, SykiError> ToSchedules()
        {
            var result = schedulesIn.ConvertAll(h => Schedule.New(h.Day, h.Start, h.End));
            foreach (var schedule in result)
            {
                if (schedule.IsError) return schedule.Error;
            }

            var schedules = result.ConvertAll(x => x.Success);

            for (int i = 0; i < schedules.Count-1; i++)
            {
                for (int j = i+1; j < schedules.Count; j++)
                {
                    if (schedules[i].Conflict(schedules[j]))
                        return new ConflictingSchedules();
                }
            }

            return result.ConvertAll(x => x.Success);
        }
    }
}
