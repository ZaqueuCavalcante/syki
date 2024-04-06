using Syki.Back.CreateAluno;
using Syki.Back.CreateCurso;
using Syki.Back.CreateOferta;
using Syki.Back.CreateAcademicPeriod;
using Syki.Back.CreateProfessor;
using Syki.Back.CreateNotification;
using Syki.Back.CreateDisciplina;
using Syki.Back.Features.Academico.CreateCampus;

namespace Syki.Back.CreateInstitution;

public class Faculdade
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public List<AcademicPeriod> AcademicPeriods { get; set; }
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
