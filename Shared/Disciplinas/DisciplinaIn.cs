namespace Syki.Shared;

public class DisciplinaIn
{
    public string Nome { get; set; }
    public ushort CargaHoraria { get; set; }
    public List<Guid> Cursos { get; set; } = new();
}
