namespace Syki.Shared;

public class GradeIn
{
    public string Nome { get; set; }
    public Guid CursoId { get; set; }
    public List<GradeDisciplinaIn> Disciplinas { get; set; } = [];
}
