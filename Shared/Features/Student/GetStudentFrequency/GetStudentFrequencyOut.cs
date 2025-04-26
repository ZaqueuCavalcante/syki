namespace Syki.Shared;

public class GetStudentFrequencyOut
{
    public int Presences { get; set; }
    public int Absences { get; set; }
    public int Attendances { get; set; }
    public int TotalLessons { get; set; }
    public decimal Frequency { get; set; }

    public string GetLessonsAsFraction()
    {
        return $"{Attendances.ToThousandSeparated()} / {TotalLessons.ToThousandSeparated()}";
    }
}
