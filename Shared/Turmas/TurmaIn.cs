namespace Syki.Shared;

public class TurmaIn
{
    public Guid OfertaId { get; set; }
    public Guid DisciplinaId { get; set; }
    public Guid ProfessorId { get; set; }
    public string Periodo { get; set; }
    public Dia Dia { get; set; }
    public Hora Start { get; set; }
    public Hora End { get; set; }

    public TurmaIn() {}

    public TurmaIn(
        Guid ofertaId,
        Guid disciplinaId,
        Guid professorId,
        string periodo,
        Dia dia,
        Hora start,
        Hora end
    ) {
        OfertaId = ofertaId;
        DisciplinaId = disciplinaId;
        ProfessorId = professorId;
        Periodo = periodo;
        Dia = dia;
        Start = start;
        End = end;
    }
}
