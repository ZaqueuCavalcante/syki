using Estud.Back.Domain.Campi;
using Estud.Back.Domain.Identity;
using Estud.Back.Domain.Disciplines;

namespace Estud.Back.Domain.Teachers;

/// <summary>
/// Representa um Professor.
/// </summary>
public class EstudTeacher
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

    public EstudUser? User { get; set; }

    private EstudTeacher() { }

    public EstudTeacher(
        EstudUser user,
        int institutionId,
        string name
    ) {
        User = user;
        InstitutionId = institutionId;
        Name = name;
    }
}
