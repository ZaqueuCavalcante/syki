using Syki.Dtos;

namespace Syki.Domain;

public class Curso
{
    public long Id { get; set; }

    public long FaculdadeId { get; set; }

    public string Nome { get; set; }

    public TipoDeCurso Tipo { get; set; }

    public Turno Turno { get; set; }

    public List<Grade> Grades { get; set; }

    public Curso() { }

    public Curso(
        string nome,
        TipoDeCurso tipo,
        long faculdadeId
    ) {
        Nome = nome;
        Tipo = tipo;
        FaculdadeId = faculdadeId;
    }
}
