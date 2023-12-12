using Syki.Shared;

namespace Syki.Back.Domain;

public class Campus
{
    public Guid Id { get; set; }
    public Guid FaculdadeId { get; set; }
    public string Nome { get; set; }
    public string Cidade { get; set; }

    public Campus(Guid faculdadeId, string nome, string cidade)
    {
        Id = Guid.NewGuid();
        FaculdadeId = faculdadeId;
        Nome = nome;
        Cidade = cidade;
    }

    public void Update(string nome, string cidade)
    {
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
