namespace Syki.Shared;

public class GetAcademicClassOut
{
    public Guid Id { get; set; }
    public string Discipline { get; set; }
    public string Code { get; set; }
    public string Teacher { get; set; }
    public string Period { get; set; }
    public int Vacancies { get; set; }
    public ClassStatus Status { get; set; }
    public List<ScheduleOut> Schedules { get; set; }
    public List<ClassLessonOut> Lessons { get; set; } = [];
    public List<AcademicClassStudentOut> Students { get; set; } = [];
    public string SchedulesInline { get; set; }
    public string FillRatio { get; set; }
    public string Workload { get; set; }
    public string Progress { get; set; }
    public decimal Frequency { get; set; }

    public static IEnumerable<(string, GetAcademicClassOut)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((GetAcademicClassOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }

    public static implicit operator GetAcademicClassOut(OneOf<GetAcademicClassOut, ErrorOut> value)
    {
        return value.Success;
    }
}
