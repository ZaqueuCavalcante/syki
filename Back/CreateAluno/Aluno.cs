using Syki.Back.CreateUser;
using Syki.Back.CreateOferta;

namespace Syki.Back.CreateAluno;

public class Aluno
{
    public Guid Id { get; }
    public Guid InstitutionId { get; }
    public SykiUser User { get; }
    public Guid OfertaId { get; }
    public Oferta Oferta { get; set; }
    public string Nome { get; private set; }
    public string Matricula { get; }

    public Aluno(
        Guid id,
        Guid institutionId,
        string nome,
        Guid ofertaId
    ) {
        Id = id;
        InstitutionId = institutionId;
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
