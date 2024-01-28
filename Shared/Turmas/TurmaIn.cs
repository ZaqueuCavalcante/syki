namespace Syki.Shared;

public class TurmaIn
{
    public Guid DisciplinaId { get; set; }
    public Guid ProfessorId { get; set; }
    public string Periodo { get; set; }
    public Dia Dia { get; set; }
    public List<Hora> Horas { get; set; }

    public TurmaIn() {}

    public TurmaIn(
        Guid disciplinaId,
        Guid professorId,
        string periodo,
        Dia dia,
        List<Hora> horas
    ) {
        DisciplinaId = disciplinaId;
        ProfessorId = professorId;
        Periodo = periodo;
        Dia = dia;
        Horas = horas;
    }
}
