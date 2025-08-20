namespace Syki.Shared;

public class GetCampusClassesOut
{
    public Guid Id { get; set; }
    public Guid DisciplineId { get; set; }
    public string Discipline { get; set; }
    public int Vacancies { get; set; }
    public int Workload { get; set; }
    public ClassStatus Status { get; set; }
    public Guid? TeacherId { get; set; }
    public string Teacher { get; set; }
}
