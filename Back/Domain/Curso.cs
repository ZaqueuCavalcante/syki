using Syki.Shared;

namespace Syki.Back.Domain;

public class Curso
{
    public Guid Id { get; set; }

    public Guid FaculdadeId { get; set; }

    public string Nome { get; set; }

    public TipoDeCurso Tipo { get; set; }

    public List<Disciplina> Disciplinas { get; set; }
    public List<CursoDisciplina> Vinculos { get; set; }

    public List<Grade> Grades { get; set; }

    public Curso() { }

    public Curso(
        string nome,
        TipoDeCurso tipo
    ) {
        Id = Guid.NewGuid();
        Nome = nome;
        Tipo = tipo;
    }

    public Curso(
        string nome,
        TipoDeCurso tipo,
        Guid faculdadeId
    ) {
        Id = Guid.NewGuid();
        Nome = nome;
        Tipo = tipo;
        FaculdadeId = faculdadeId;
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
