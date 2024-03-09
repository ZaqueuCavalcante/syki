namespace Syki.Back.CreateCurso;

public class Curso
{
    public Guid Id { get; set; }
    public Guid FaculdadeId { get; set; }
    public string Nome { get; set; }
    public TipoDeCurso Tipo { get; set; }
    public List<Disciplina> Disciplinas { get; set; }
    public List<CursoDisciplina> Vinculos { get; set; }
    public List<Grade> Grades { get; set; }

    public Curso(Guid faculdadeId, string nome, TipoDeCurso tipo)
    {
        Id = Guid.NewGuid();
        FaculdadeId = faculdadeId;
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
