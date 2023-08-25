namespace Syki.Domain;

public class Turma
{
    public long Id { get; set; }
    
    public long ProfessorId { get; set; }
    public long DisciplinaId { get; set; }
    public long CursoId { get; set; }

    public List<Aluno> Alunos { get; set; }
}
