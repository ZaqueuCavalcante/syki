using Syki.Back.Domain.Campi;
using Syki.Back.Domain.Disciplines;

namespace Syki.Back.Domain.Teachers;

/// <summary>
/// Representa um Professor.
/// </summary>
public class SykiTeacher
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public string Name { get; set; }

    /// <summary>
    /// Disciplinas que o professor está apto a lecionar
    /// </summary>
    public List<Discipline> Disciplines { get; set; }

    /// <summary>
    /// Conjunto de campus que o professor trabalha
    /// </summary>
    public List<Campus> Campi { get; set; }

    private SykiTeacher() { }

    public SykiTeacher(
        int userId,
        int institutionId,
        string name
    ) {
        Id = userId;
        InstitutionId = institutionId;
        Name = name;
    }
}
