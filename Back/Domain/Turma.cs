using Syki.Shared;

namespace Syki.Back.Domain;

public class Turma
{
    public Guid Id { get; set; }
    public Guid FaculdadeId { get; set; }
    public Guid DisciplinaId { get; set; }
    public Disciplina Disciplina { get; set; }
    public Guid ProfessorId { get; set; }
    public Professor Professor { get; set; }
    public string Periodo { get; set; }
    public List<Aula> Aulas { get; set; }
    public List<Aluno> Alunos { get; set; }
    public List<Horario> Horarios { get; set; }

    private Turma() {}

    public Turma(
        Guid faculdadeId,
        Guid disciplinaId,
        Guid professorId,
        string periodo,
        List<Horario> horarios
    ) {
        Id = Guid.NewGuid();
        FaculdadeId = faculdadeId;
        DisciplinaId = disciplinaId;
        ProfessorId = professorId;
        Periodo = periodo;
        Horarios = horarios;
    }

    public TurmaOut ToOut()
    {
        var horario = Horarios.First();
        return new TurmaOut
        {
            Id = Id,
            Disciplina = Disciplina.Nome,
            Professor = Professor.Nome,
            Periodo = Periodo,
            Dia = horario.Dia,
            Start = horario.Start,
            End = horario.End,
        };
    }
}
