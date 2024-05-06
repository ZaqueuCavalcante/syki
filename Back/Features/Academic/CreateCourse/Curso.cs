using Syki.Back.Features.Academico.CreateGrade;
using Syki.Back.Features.Academico.CreateDisciplina;

namespace Syki.Back.Features.Academico.CreateCurso;

public class Curso
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }
    public CourseType Type { get; set; }
    public List<Disciplina> Disciplinas { get; set; }
    public List<CursoDisciplina> Vinculos { get; set; }
    public List<Grade> Grades { get; set; }

    public Curso(Guid institutionId, string name, CourseType type)
    {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        Name = name;
        Type = type;
    }

    public CourseOut ToOut()
    {
        return new CourseOut
        {
            Id = Id,
            Name = Name,
            Type = Type,
        };
    }
}
