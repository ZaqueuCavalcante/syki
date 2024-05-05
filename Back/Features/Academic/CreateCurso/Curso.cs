using Syki.Back.Features.Academico.CreateDisciplina;
using Syki.Back.Features.Academico.CreateGrade;

namespace Syki.Back.Features.Academico.CreateCurso;

public class Curso
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }
    public TipoDeCurso Tipo { get; set; }
    public List<Disciplina> Disciplinas { get; set; }
    public List<CursoDisciplina> Vinculos { get; set; }
    public List<Grade> Grades { get; set; }

    public Curso(Guid institutionId, string name, TipoDeCurso tipo)
    {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        Name = name;
        Tipo = tipo;
    }

    public CursoOut ToOut()
    {
        return new CursoOut
        {
            Id = Id,
            Name = Name,
            Tipo = Tipo,
        };
    }
}
