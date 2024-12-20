namespace Syki.Back.Features.Academic.CreateTeacher;

/// <summary>
/// Representa um Professor.
/// </summary>
public class SykiTeacher : Entity
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }

    private SykiTeacher() {}

    public SykiTeacher(
        Guid userId,
        Guid institutionId,
        string name
    ) {
        Id = userId;
        InstitutionId = institutionId;
        Name = name;

        AddDomainEvent(new TeacherCreatedDomainEvent(Id, InstitutionId));
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
