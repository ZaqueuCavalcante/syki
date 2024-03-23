namespace Syki.Back.Domain;

public class Aula
{
    public Guid Id { get; set; }
    public Guid TurmaId { get; set; }
    public DateOnly Day { get; set; }
    public Hora Start { get; set; }
    public Hora End { get; set; }

    public Aula(
        Guid turmaId,
        DateOnly day,
        Hora start,
        Hora end
    ) {
        Id = Guid.NewGuid();
        TurmaId = turmaId;
        Day = day;
        Start = start;
        End = end;
    }
}
