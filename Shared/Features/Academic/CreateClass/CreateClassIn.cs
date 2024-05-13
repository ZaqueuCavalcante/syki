namespace Syki.Shared;

public class CreateClassIn
{
    public Guid DisciplineId { get; set; }
    public Guid TeacherId { get; set; }
    public string Period { get; set; }
    public List<ScheduleIn> Schedules { get; set; }

    public CreateClassIn() {}

    public CreateClassIn(
        Guid disciplineId,
        Guid teacherId,
        string period,
        List<ScheduleIn> schedules
    ) {
        DisciplineId = disciplineId;
        TeacherId = teacherId;
        Period = period;
        Schedules = schedules;
    }
}
