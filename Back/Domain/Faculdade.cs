using Syki.Back.CreateAluno;
using Syki.Back.CreateCampus;

namespace Syki.Back.Domain;

public class Faculdade
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public List<Periodo> Periodos { get; set; }
    public List<Campus> Campi { get; set; }
    public List<Curso> Cursos { get; set; }
    public List<Oferta> Ofertas { get; set; }
    public List<Grade> Grades { get; set; }
    public List<Disciplina> Disciplinas { get; set; }
    public List<Professor> Professores { get; set; }
    public List<Aluno> Alunos { get; set; }
    public List<Notification> Notifications { get; set; }

    public Faculdade() { }

    public Faculdade(string nome)
    {
        Id = Guid.NewGuid();
        Nome = nome;
    }

    public FaculdadeOut ToOut()
    {
        return new FaculdadeOut
        {
            Id = Id,
            Nome = Nome,
        };
    }
}
