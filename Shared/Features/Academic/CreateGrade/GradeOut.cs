namespace Syki.Shared;

public class GradeOut
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public string CursoNome { get; set; }
    public string Name { get; set; }
    public List<DisciplinaOut> Disciplinas { get; set; }
}
