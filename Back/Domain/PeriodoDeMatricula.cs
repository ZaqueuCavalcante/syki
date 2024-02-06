using Syki.Back.Exceptions;

namespace Syki.Back.Domain;

public class PeriodoDeMatricula
{
    public string Id { get; set; }
    public Guid FaculdadeId { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }

    public PeriodoDeMatricula(
        string id,
        Guid faculdadeId,
        DateOnly start,
        DateOnly end
    ) {
        Id = id;
        FaculdadeId = faculdadeId;

        if (start >= end)
            Throw.DE1101.Now();

        Start = start;
        End = end;
    }
}
