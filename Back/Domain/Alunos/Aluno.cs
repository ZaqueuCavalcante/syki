using Syki.Shared;
using Syki.Back.Exceptions;
using Syki.Back.CreateUser;

namespace Syki.Back.Domain;

public class Aluno
{
    public Guid Id { get; set; }
    public Guid FaculdadeId { get; set; }
    public SykiUser User { get; set; }
    public Guid OfertaId { get; set; }
    public Oferta Oferta { get; set; }
    public string Nome { get; set; }
    public string Matricula { get; set; }

    public Aluno(
        Guid id,
        Guid faculdadeId,
        string nome,
        Guid ofertaId
    ) {
        Id = id;
        FaculdadeId = faculdadeId;
        OfertaId = ofertaId;
        SetNome(nome);
        Matricula = $"{DateTime.Now.Year}{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    private void SetNome(string nome)
    {
        if (nome.IsEmpty() || nome.Length < 3)
        {
            Throw.DE000.Now();
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
            Email = User?.Email ?? "-",
            Nome = Nome,
            Matricula = Matricula,
        };
    }
}
