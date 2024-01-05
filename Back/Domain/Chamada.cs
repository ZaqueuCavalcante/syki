namespace Syki.Back.Domain;

public class Chamada
{
    public Guid AulaId { get; set; }
    public Guid AlunoId { get; set; }
    public bool Presente { get; set; }

    public Chamada(
        Guid aulaId,
        Guid alunoId,
        bool presente
    ) {
        AulaId = aulaId;
        AlunoId = alunoId;
        Presente = presente;
    }
}
