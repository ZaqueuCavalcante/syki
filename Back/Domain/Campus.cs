namespace Syki.Domain;

public class Campus
{
    public long Id { get; set; }
    
    public string Nome { get; set; }

    public long FaculdadeId { get; set; }

    public Campus(string nome, long faculdadeId)
    {
        Nome = nome;
        FaculdadeId = faculdadeId;
    }
}
