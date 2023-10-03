using Syki.Dtos;

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

    public List<Aluno> Alunos { get; set; }

    public List<Aula> Aulas { get; set; }

    public TurmaOut ToOut()
    {
        return new TurmaOut
        {
            Disciplina = Disciplina.Nome,
            Professor = Professor.Nome,
            Periodo = Periodo,
        };
    }
}
