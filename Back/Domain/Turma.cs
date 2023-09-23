using Syki.Dtos;

namespace Syki.Domain;

public class Turma
{
    public long Id { get; set; }

    public long FaculdadeId { get; set; }

    public long DisciplinaId { get; set; }
    public Disciplina Disciplina { get; set; }

    public long ProfessorId { get; set; }
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
