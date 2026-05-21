using Syki.Back.Domain.Campi;
using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Disciplines;

namespace Syki.Back.Domain.Teachers;

/// <summary>
/// Representa um Professor.
/// </summary>
public class SykiTeacher
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }

    /// <summary>
    /// Disciplinas que o professor está apto a lecionar
    /// </summary>
    public List<Discipline> Disciplines { get; set; }

    /// <summary>
    /// Conjunto de campus que o professor trabalha
    /// </summary>
    public List<Campus> Campi { get; set; }

    public SykiUser? User { get; set; }

    private SykiTeacher() { }

    public SykiTeacher(
        SykiUser user,
        int institutionId,
        string name
    ) {
        User = user;
        InstitutionId = institutionId;
        Name = name;
    }
}
