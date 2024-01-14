namespace Syki.Back.Domain;

public class Aula
{
    public Guid Id { get; set; }
    public Guid TurmaId { get; set; }
    // public AulaStatus Status { get; set; } // Dada, Marcada, Cancelada...
    public DateTime Start { get; set; }
    public DateTime End { get; set; } // Precisa? Todas tem a mesma duracao...

    public Aula(
        Guid turmaId,
        DateTime start,
        DateTime end
    ) {
        Id = Guid.NewGuid();
        TurmaId = turmaId;
        Start = start;
        End = end;
    }
}
