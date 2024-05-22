namespace Syki.Back.Features.Academic.CreateTeacher;

public class SykiTeacher
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }

    public SykiTeacher(
        Guid id,
        Guid institutionId,
        string name
    ) {
        Id = id;
        InstitutionId = institutionId;
        SetName(name);
    }

    private void SetName(string name)
    {
        if (name.IsEmpty() || name.Length < 3)
            Throw.DE001.Now();

        Name = name;
    }

    public TeacherOut ToOut()
    {
        return new TeacherOut
        {
            Id = Id,
            Name = Name,
        };
    }
}
