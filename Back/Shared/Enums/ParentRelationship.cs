namespace Estud.Back.Shared;

/// <summary>
/// Parentesco entre o Responsável e o Aluno
/// </summary>
public enum ParentRelationship
{
    [Description("Mãe")]
    Mother = 0,

    [Description("Pai")]
    Father = 1,

    [Description("Responsável legal")]
    Guardian = 2,

    [Description("Outro")]
    Other = 3,
}
