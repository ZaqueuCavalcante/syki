namespace Syki.Back.Domain;

public class Aula
{
    public Guid Id { get; set; }
    public Guid TurmaId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

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
