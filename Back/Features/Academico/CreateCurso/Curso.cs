using Syki.Back.Features.Academico.CreateDisciplina;
using Syki.Back.Features.Academico.CreateGrade;

namespace Syki.Back.Features.Academico.CreateCurso;

public class Curso
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Nome { get; set; }
    public TipoDeCurso Tipo { get; set; }
    public List<Disciplina> Disciplinas { get; set; }
    public List<CursoDisciplina> Vinculos { get; set; }
    public List<Grade> Grades { get; set; }

    public Curso(Guid institutionId, string nome, TipoDeCurso tipo)
    {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        Nome = nome;
        Tipo = tipo;
    }

    public CursoOut ToOut()
    {
        return new CursoOut
        {
            Id = Id,
            Nome = Nome,
            Tipo = Tipo,
        };
    }
}
