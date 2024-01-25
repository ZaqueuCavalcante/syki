using Syki.Shared;
using Syki.Back.Exceptions;

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
        SetNome(nome);
    }

    private void SetNome(string nome)
    {
        if (nome.IsEmpty() || nome.Length < 3)
        {
            throw DomainExceptions.DE0000();
        }

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
