using Estud.Back.Domain.Campi;
using Estud.Back.Domain.Classes;

namespace Estud.Back.Domain.Classrooms;

/// <summary>
/// Sala de Aula
/// </summary>
public class Classroom
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }

    public int CampusId { get; set; }
    public Campus Campus { get; set; }

    public string Name { get; set; }
    public int Capacity { get; set; }

    public List<Schedule> Schedules { get; set; }

    private Classroom() { }

    public Classroom(
        int institutionId,
        int campusId,
        string name,
        int capacity
    ) {
        InstitutionId = institutionId;
        CampusId = campusId;
        Name = name;
        Capacity = capacity;
    }

    public void Update(int campusId, string name, int capacity)
    {
        CampusId = campusId;
        Name = name;
        Capacity = capacity;
    }
}
