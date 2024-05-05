namespace Syki.Shared;

public class GradeIn
{
    public string Name { get; set; }
    public Guid CursoId { get; set; }
    public List<GradeDisciplinaIn> Disciplinas { get; set; } = [];
}
