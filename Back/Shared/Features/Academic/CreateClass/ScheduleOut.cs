namespace Estud.Back.Shared;

public class ScheduleOut
{
    public Day Day { get; set; }
    public Hour StartAt { get; set; }
    public Hour EndAt { get; set; }

    /// <summary>
    /// Professor que cobre este horário.
    /// Pode ser nulo quando a turma ainda não tem professor definido.
    /// </summary>
    public int? TeacherId { get; set; }
    public string? Teacher { get; set; }

    /// <summary>
    /// Sala que sedia este horário.
    /// Pode ser nula quando a turma ainda não tem sala definida.
    /// </summary>
    public int? ClassroomId { get; set; }
    public string? Classroom { get; set; }

    public ScheduleOut() { }

    public ScheduleOut(
        Day day,
        Hour startAt,
        Hour endAt
    ) {
        Day = day;
        StartAt = startAt;
        EndAt = endAt;
    }
}
