using Syki.Back.CreateAluno;
using Syki.Back.CreateOferta;
using Syki.Back.CreateProfessor;
using Syki.Back.CreateNotification;
using Syki.Back.CreateAcademicPeriod;
using Syki.Back.Features.Academico.CreateCurso;
using Syki.Back.Features.Academico.CreateGrade;
using Syki.Back.Features.Academico.CreateCampus;
using Syki.Back.Features.Academico.CreateDisciplina;

namespace Syki.Back.CreateInstitution;

public class Institution
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

    public Institution() { }

    public Institution(string nome)
    {
        Id = Guid.NewGuid();
        Nome = nome;
    }

    public InstitutionOut ToOut()
    {
        return new InstitutionOut
        {
            Id = Id,
            Name = Nome,
        };
    }
}
