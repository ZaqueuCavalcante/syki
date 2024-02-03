namespace Syki.Shared;

public class TurmaIn
{
    public Guid DisciplinaId { get; set; }
    public Guid ProfessorId { get; set; }
    public string Periodo { get; set; }
    public Dia Dia { get; set; }
    public Hora Start { get; set; }
    public Hora End { get; set; }
    public List<HorarioIn> Horarios { get; set; }

    public TurmaIn() {}

    public TurmaIn(
        Guid disciplinaId,
        Guid professorId,
        string periodo,
        List<HorarioIn> horarios
    ) {
        DisciplinaId = disciplinaId;
        ProfessorId = professorId;
        Periodo = periodo;
        Horarios = horarios;
    }
}
