using Syki.Shared;
using Syki.Back.Exceptions;

namespace Syki.Back.Domain;

public class Aluno
{
    public Guid Id { get; set; }
    
    public Guid FaculdadeId { get; set; }

    public Guid? UserId { get; set; }

    public Guid OfertaId { get; set; }
    public Oferta Oferta { get; set; }

    public string Nome { get; set; }

    public string Matricula { get; set; }

    public Aluno(Guid faculdadeId, string nome)
    {
        Id = Guid.NewGuid();
        FaculdadeId = faculdadeId;
        SetNome(nome);
        Matricula = $"{DateTime.Now.Year}{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    private void SetNome(string nome)
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
            OfertaId = OfertaId,
            Oferta = Oferta?.Curso?.Nome ?? "-",
            Nome = Nome,
            Matricula = Matricula,
        };
    }
}
