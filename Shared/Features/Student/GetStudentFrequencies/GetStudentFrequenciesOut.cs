namespace Syki.Shared;

public class GetStudentFrequenciesOut
{
    public string Name { get; set; }
    public string Period { get; set; }
    public StudentDisciplineStatus Status { get; set; }
    public int Lessons { get; set; }
    public int Presences { get; set; }

    public GetStudentFrequenciesOut() {}

    public GetStudentFrequenciesOut(string name, string period, int lessons, int presences, StudentDisciplineStatus status)
    {
        Name = name;
        Period = period;
        Lessons = lessons;
        Presences = presences;
        Status = status;
    }
    
    public string GetFormated()
    {
        return $"{Presences.Format()} / {Lessons.Format()}";
    }

    public decimal GetPercentage()
    {
        if (Lessons == 0) return 0.00M;
        return 100M*(1M * Presences / (1M * Lessons));
    }
}
