namespace Syki.Domain;

public class Faculdade
{
    public long Id { get; set; }
    
    public string Nome { get; set; }

    public List<Campus> Campi { get; set; }

    public List<Curso> Cursos { get; set; }

    public List<Grade> Grades { get; set; }

    public List<Disciplina> Disciplinas { get; set; }

    public List<Professor> Professores { get; set; }

    public List<Aluno> Alunos { get; set; }

    public Faculdade() { }

    public Faculdade(string nome)
    {
        Nome = nome;
    }
}
