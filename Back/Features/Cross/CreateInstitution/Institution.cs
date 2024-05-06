using Syki.Back.CreateOferta;
using Syki.Back.CreateProfessor;
using Syki.Back.CreateNotification;
using Syki.Back.Features.Academic.CreateCurso;
using Syki.Back.Features.Academic.CreateGrade;
using Syki.Back.Features.Academic.CreateCampus;
using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Academic.CreateDisciplina;
using Syki.Back.Features.Academic.CreateAcademicPeriod;

namespace Syki.Back.CreateInstitution;

public class Institution
{
    public Guid Id { get; set; }
    public string Name { get; set; }
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

    public Institution(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public InstitutionOut ToOut()
    {
        return new InstitutionOut
        {
            Id = Id,
            Name = Name,
        };
    }
}
