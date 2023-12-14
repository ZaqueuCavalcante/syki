using Syki.Shared;

namespace Syki.Back.Domain;

public class Professor
{
    public Guid Id { get; set; }
    public Guid FaculdadeId { get; set; }
    public Guid? UserId { get; set; }
    public string Nome { get; set; }

    public Professor(Guid faculdadeId, string nome)
    {
        Id = Guid.NewGuid();
        FaculdadeId = faculdadeId;
        Nome = nome;
    }

    public ProfessorOut ToOut()
    {
        return new ProfessorOut
        {
            Id = Id,
            Nome = Nome,
        };
    }
}
