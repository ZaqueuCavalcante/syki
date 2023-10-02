namespace Syki.Domain;

public class Aula
{
    public Guid Id { get; set; }
    
    public Guid TurmaId { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }
}
