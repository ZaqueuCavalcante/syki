namespace Syki.Back.Domain;

public class Campus
{
    public Guid Id { get; set; }
    
    public Guid FaculdadeId { get; set; }

    public string Nome { get; set; }

    public Campus(string nome)
    {
        Id = Guid.NewGuid();
        Nome = nome;
    }

    public Campus(string nome, Guid faculdadeId)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        FaculdadeId = faculdadeId;
    }
}
