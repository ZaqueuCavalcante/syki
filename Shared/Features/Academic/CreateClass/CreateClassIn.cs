namespace Syki.Shared;

public class CreateClassIn
{
    public Guid DisciplineId { get; set; }
    public Guid TeacherId { get; set; }
    public string Period { get; set; }
    public int Vacancies { get; set; }
    public List<ScheduleIn> Schedules { get; set; }

    public CreateClassIn() {}

    public CreateClassIn(
        Guid disciplineId,
        Guid teacherId,
        string period,
        int vacancies,
        List<ScheduleIn> schedules
    ) {
        DisciplineId = disciplineId;
        TeacherId = teacherId;
        Period = period;
        Vacancies = vacancies;
        Schedules = schedules;
    }
}
