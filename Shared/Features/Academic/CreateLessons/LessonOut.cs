namespace Syki.Shared;

public class LessonOut
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public DateOnly Date { get; set; }
    public string Schedule { get; set; }
    public LessonStatus Status { get; set; }
    public decimal Frequency { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((LessonOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }
}
