namespace Syki.Shared;

public class GetStudentClassFrequencyOut
{
    public int Presences { get; set; }
    public int Attendances { get; set; }
    public decimal Frequency { get; set; }

    public double GetFrequencyAsDouble()
    {
        return decimal.ToDouble(Frequency);
    }

    public string GetFrequencyAsFraction()
    {
        return $"{Presences} / {Attendances}";
    }
}
