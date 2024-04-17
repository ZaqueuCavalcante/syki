namespace Syki.Back.CreateProfessor;

public class Professor
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Nome { get; set; }

    public Professor(
        Guid id,
        Guid institutionId,
        string nome
    ) {
        Id = id;
        InstitutionId = institutionId;
        SetNome(nome);
    }

    private void SetNome(string nome)
    {
        if (nome.IsEmpty() || nome.Length < 3)
        {
            Throw.DE001.Now();
        }

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
