namespace Syki.Domain;

public class Aula
{
    public long Id { get; set; }
    
    public long TurmaId { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }
}
