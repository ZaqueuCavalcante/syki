using Estud.Back.Domain.Identity;

namespace Estud.Back.Domain.Parents;

/// <summary>
/// Representa um Responsável (mãe, pai ou responsável legal de alunos).
/// </summary>
public class EstudParent
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }

    public EstudUser? User { get; set; }

    private EstudParent() {}

    public EstudParent(
        EstudUser user,
        int institutionId,
        string name
    ) {
        User = user;
        InstitutionId = institutionId;
        Name = name;
    }
}
