using Syki.Back.CreateOferta;
using Syki.Back.CreateProfessor;
using Syki.Back.Features.Academic.CreateNotification;
using Syki.Back.Features.Academic.CreateCurso;
using Syki.Back.Features.Academic.CreateGrade;
using Syki.Back.Features.Academic.CreateCampus;
using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Academic.CreateDisciplina;
using Syki.Back.Features.Academic.CreateAcademicPeriod;

namespace Syki.Back.Features.Cross.CreateInstitution;

public class Institution
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<AcademicPeriod> AcademicPeriods { get; set; }
    public List<Campus> Campi { get; set; }
    public List<Curso> Courses { get; set; }
    public List<Oferta> CourseOfferings { get; set; }
    public List<Grade> CourseCurriculums { get; set; }
    public List<Disciplina> Disciplines { get; set; }
    public List<Professor> Teachers { get; set; }
    public List<Aluno> Students { get; set; }
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
