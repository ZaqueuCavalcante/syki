using Syki.Shared;

namespace Syki.Back.Domain;

public class Professor
{
    public Guid Id { get; set; }
    public Guid FaculdadeId { get; set; }
    public Guid UserId { get; set; }
    public string Nome { get; set; }

    public Professor(
        Guid faculdadeId,
        Guid userId,
        string nome
    ) {
        Id = Guid.NewGuid();
        FaculdadeId = faculdadeId;
        UserId = userId;
        Nome = nome;
    }

    public ProfessorOut ToOut()
    {
        return new ProfessorOut
        {
            Id = Id,
            UserId = UserId,
            Nome = Nome,
        };
    }
}
