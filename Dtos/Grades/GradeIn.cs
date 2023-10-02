namespace Syki.Dtos;

public class GradeIn
{
    public string Nome { get; set; }
    public Guid CursoId { get; set; }
    public List<GradeDisciplinaIn> Disciplinas { get; set; }
}

public class GradeDisciplinaIn
{
    public Guid Id { get; set; }
    public byte Periodo { get; set; }
    public byte Creditos { get; set; }
    public ushort CargaHoraria { get; set; }
}
