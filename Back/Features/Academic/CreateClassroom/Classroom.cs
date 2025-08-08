using Syki.Back.Features.Academic.CreateCampus;

namespace Syki.Back.Features.Academic.CreateClassroom;

/// <summary>
/// Sala de Aula
/// </summary>
public class Classroom
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public Guid CampusId { get; set; }
    public Campus Campus { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }

    private Classroom() { }

    public Classroom(
        Guid institutionId,
        Guid campusId,
        string name,
        int capacity
    ) {
        Id = Guid.CreateVersion7();
        InstitutionId = institutionId;
        CampusId = campusId;
        Name = name;
        Capacity = capacity;
    }

    public CreateClassroomOut ToCreateClassroomOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
        };
    }

    public GetClassroomsOut ToGetClassroomsOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Campus = Campus.Name,
            Capacity = Capacity,
        };
    }
}
