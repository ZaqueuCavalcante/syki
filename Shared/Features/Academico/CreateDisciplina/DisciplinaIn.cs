namespace Syki.Shared;

public class DisciplinaIn
{
    public string Nome { get; set; }
    public string Code { get; set; }
    public List<Guid> Cursos { get; set; } = [];
}
