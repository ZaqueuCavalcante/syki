namespace Syki.Back.Domain;

public class Chamada
{
    public Guid AulaId { get; set; }
    
    public Guid AlunoId { get; set; }

    public bool Presente { get; set; }
}
