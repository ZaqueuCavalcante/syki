using Syki.Back.CreateAluno;
using Syki.Back.CreateProfessor;
using Syki.Back.CreateDisciplina;
using Syki.Back.CreateEvaluationUnits;

namespace Syki.Back.CreateTurma;

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
    public List<EvaluationUnit> Units { get; set; } = [];

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
        SetHorarios(horarios);
    }

    private void SetHorarios(List<Horario> horarios)
    {
        for (int i = 0; i < horarios.Count-1; i++)
        {
            for (int j = i+1; j < horarios.Count; j++)
            {
                if (horarios[i].Conflict(horarios[j]))
                    Throw.DE022.Now();
            }
        }

        Horarios = horarios;
    }

    public string GetHorarioAsString()
    {
        return string.Join(" | ", Horarios.OrderBy(h => h.Dia).ThenBy(h => h.Start).ToList().ConvertAll(h => h.ToString()));
    }

    public TurmaOut ToOut()
    {
        return new TurmaOut
        {
            Id = Id,
            Disciplina = Disciplina.Nome,
            Professor = Professor.Nome,
            Periodo = Periodo,
            Horarios = Horarios.ConvertAll(h => h.ToOut()),
            HorariosInline = GetHorarioAsString(),
        };
    }

    public ProfessorTurmaOut ToProfessorTurmaOut()
    {
        return new ProfessorTurmaOut
        {
            Id = Id,
            Disciplina = Disciplina.Nome,
            Code = Disciplina.Code,
            Periodo = Periodo,
            Horarios = Horarios.ConvertAll(h => h.ToOut()),
            HorariosInline = GetHorarioAsString(),
        };
    }
}
