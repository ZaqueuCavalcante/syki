namespace Syki.Dtos;

public class GradeOut
{
    public long Id { get; set; }
    public long CursoId { get; set; }
    public string CursoNome { get; set; }
    public string Nome { get; set; }
    public List<DisciplinaOut> Disciplinas { get; set; }
}
