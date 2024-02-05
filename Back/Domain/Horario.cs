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

    public bool Conflict(Horario other)
    {
        if (Dia != other.Dia)
            return false;

        if (Start == other.Start || End == other.End)
            return true;

        if (Start < other.Start && other.Start < End)
            return true;

        if (Start < other.End && other.End < End)
            return true;

        if (other.Start < Start && Start < other.End)
            return true;

        if (other.Start < End && End < other.End)
            return true;

        return false;
    }

    public override string ToString()
    {
        return $"{Dia.GetDescription()} {Start.GetDescription()}-{End.GetDescription()}";
    }

    public HorarioOut ToOut()
    {
        return new HorarioOut
        {
            Dia = Dia,
            Start = Start,
            End = End,
        };
    }
}
