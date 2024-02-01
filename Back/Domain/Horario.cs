using Syki.Shared;
using Syki.Back.Exceptions;

namespace Syki.Back.Domain;

public class Horario
{
    public Guid Id { get; set; }
    public Guid TurmaId { get; set; }
    public Dia Dia { get; set; }
    public Hora Start { get; set; }
    public Hora End { get; set; }

    public Horario(
        Dia dia,
        Hora start,
        Hora end
    ) {
        Id = Guid.NewGuid();
        Dia = dia;

        if (start == end || end < start)
            Throw.DE0018.Now();

        Start = start;
        End = end;
    }
}
