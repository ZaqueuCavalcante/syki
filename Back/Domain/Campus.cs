using Syki.Shared;

namespace Syki.Back.Domain;

public class Campus
{
    public Guid Id { get; set; }
    
    public Guid FaculdadeId { get; set; }

    public string Nome { get; set; }

    public string Cidade { get; set; }

    public Campus(string nome, string cidade)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Cidade = cidade;
    }

    public Campus(string nome, string cidade, Guid faculdadeId)
    {
        Id = Guid.NewGuid();
        FaculdadeId = faculdadeId;
        Nome = nome;
        Cidade = cidade;
    }

    public CampusOut ToOut()
    {
        return new CampusOut
        {
            Id = Id,
            Nome = Nome,
            Cidade = Cidade,
        };
    }
}
