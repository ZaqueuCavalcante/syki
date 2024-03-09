namespace Syki.Back.CreateDisciplina;

public class Disciplina
{
    public Guid Id { get; set; }
    public Guid FaculdadeId { get; set; }
    public string Nome { get; set; }
    public List<CursoDisciplina> Vinculos { get; set; }

    public Disciplina(
        Guid faculdadeId,
        string nome
    ) {
        Id = Guid.NewGuid();
        FaculdadeId = faculdadeId;
        Nome = nome;
        Vinculos = [];
    }

    public DisciplinaOut ToOut()
    {
        return new DisciplinaOut
        {
            Id = Id,
            Nome = Nome,
            Cursos = Vinculos.ConvertAll(v => v.CursoId),
        };
    }
}
