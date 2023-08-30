namespace Syki.Domain;

public class Grade
{
    public long Id { get; set; }
    
    public long FaculdadeId { get; set; }

    public string Nome { get; set; }

    public List<Disciplina> Disciplinas { get; set; }
}
