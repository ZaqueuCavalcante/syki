namespace Syki.Dtos;

public class TurmaIn
{
    public Guid DisciplinaId { get; set; }
    public Guid ProfessorId { get; set; }
    public string Periodo { get; set; }
    public List<Guid> Ofertas { get; set; }
}
