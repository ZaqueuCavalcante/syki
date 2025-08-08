using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.CreateDiscipline;

namespace Syki.Back.Features.Academic.CreateTeacher;

/// <summary>
/// Representa um Professor.
/// </summary>
public class SykiTeacher : Entity
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }

    /// <summary>
    /// Disciplinas que o professor está apto a lecionar
    /// </summary>
    public List<Discipline> Disciplines { get; set; }

    /// <summary>
    /// Preferências de horários do professor no semestre atual
    /// </summary>
    public List<Schedule> SchedulingPreferences { get; set; }

    private SykiTeacher() { }

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

    public GetAcademicTeacherOut ToGetAcademicTeacherOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Disciplines = Disciplines.ConvertAll(x => x.ToGetAcademicTeacherDisciplineOut()),
        };
    }
}
