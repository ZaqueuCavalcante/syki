namespace Syki.Domain;

public class Turma
{
    public long Id { get; set; }
    
    public long ProfessorId { get; set; }

    public long DisciplinaId { get; set; }

    public string Periodo { get; set; }

    public List<Aluno> Alunos { get; set; }

    public List<Aula> Aulas { get; set; }
}
