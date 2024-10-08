namespace Syki.Back.Features.Academic.CreateTeacher;

/// <summary>
/// Representa um Professor.
/// </summary>
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
        Name = name;
    }

    public TeacherOut ToOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
        };
    }
}
