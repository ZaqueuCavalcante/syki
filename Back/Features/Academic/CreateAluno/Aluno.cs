using Syki.Back.CreateOferta;

namespace Syki.Back.CreateAluno;

public class Aluno
{
    public Guid Id { get; }
    public Guid InstitutionId { get; }
    public SykiUser User { get; }
    public Guid OfertaId { get; }
    public Oferta Oferta { get; set; }
    public string Name { get; private set; }
    public string Matricula { get; }

    public Aluno(
        Guid id,
        Guid institutionId,
        string name,
        Guid ofertaId
    ) {
        Id = id;
        InstitutionId = institutionId;
        OfertaId = ofertaId;
        SetNome(name);
        Matricula = $"{DateTime.Now.Year}{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    private void SetNome(string name)
    {
        if (name.IsEmpty() || name.Length < 3)
        {
            Throw.DE000.Now();
        }

        Name = name;
    }

    public AlunoOut ToOut()
    {
        return new AlunoOut
        {
            Id = Id,
            OfertaId = OfertaId,
            Oferta = Oferta?.Curso?.Name ?? "-",
            Email = User?.Email ?? "-",
            Name = Name,
            Matricula = Matricula,
        };
    }
}
