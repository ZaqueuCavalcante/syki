namespace Syki.Back.CreateProfessor;

public class Professor
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }

    public Professor(
        Guid id,
        Guid institutionId,
        string name
    ) {
        Id = id;
        InstitutionId = institutionId;
        SetNome(name);
    }

    private void SetNome(string name)
    {
        if (name.IsEmpty() || name.Length < 3)
        {
            Throw.DE001.Now();
        }

        Name = name;
    }

    public ProfessorOut ToOut()
    {
        return new ProfessorOut
        {
            Id = Id,
            Name = Name,
        };
    }
}
