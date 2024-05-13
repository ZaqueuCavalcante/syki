namespace Syki.Shared;

public class EnrollmentClassOut
{
    public Guid Id { get; set; }
    public string Discipline { get; set; }
    public byte Period { get; set; }
    public byte Credits { get; set; }
    public ushort Workload { get; set; }
    public string Teacher { get; set; }
    public List<ScheduleOut> Schedules { get; set; }
    public string SchedulesInline { get; set; }
    public string Students { get; set; }

    public bool IsSelected { get; set; }
}
