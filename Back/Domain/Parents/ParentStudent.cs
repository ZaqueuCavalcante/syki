using Estud.Back.Domain.Students;

namespace Estud.Back.Domain.Parents;

/// <summary>
/// Vínculo entre um Responsável e um Aluno.
/// </summary>
public class ParentStudent
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public int ParentId { get; set; }
    public int StudentId { get; set; }
    public ParentRelationship Relationship { get; set; }
    public ParentStudentStatus Status { get; set; }

    /// <summary>
    /// Aluno maior de idade pode revogar o acesso do responsável aos seus dados.
    /// </summary>
    public bool RevokedByStudent { get; set; }

    public DateTime CreatedAt { get; set; }

    public EstudParent? Parent { get; set; }
    public EstudStudent? Student { get; set; }

    private ParentStudent() {}

    public ParentStudent(
        int institutionId,
        EstudParent parent,
        int studentId,
        ParentRelationship relationship
    ) {
        InstitutionId = institutionId;
        Parent = parent;
        StudentId = studentId;
        Relationship = relationship;
        Status = ParentStudentStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }
}
