using Syki.Dtos;
using Syki.Exceptions;
using Syki.Extensions;

namespace Syki.Domain;

public class Aluno
{
    public Guid Id { get; set; }
    
    public Guid FaculdadeId { get; set; }

    public string Nome { get; set; }

    public string Matricula { get; set; }

    public Aluno(Guid faculdadeId, string nome)
    {
        Id = Guid.NewGuid();
        FaculdadeId = faculdadeId;
        SetNome(nome);
        Matricula = $"{DateTime.Now.Year}{Guid.NewGuid().ToString()[..8]}";
    }

    public void SetNome(string nome)
    {
        if (nome.IsEmpty() || nome.Length < 3)
        {
            throw new DomainException(ExceptionMessages.DE0000);
        }

        Nome = nome;
    }

    public AlunoOut ToOut()
    {
        return new AlunoOut
        {
            Id = Id,
            Nome = Nome,
            Matricula = Matricula,
        };
    }
}
