namespace Syki.Dtos;

public class DisciplinaIn
{
    public string Nome { get; set; }
    public ushort CargaHoraria { get; set; }
    public List<long> Cursos { get; set; }
}
