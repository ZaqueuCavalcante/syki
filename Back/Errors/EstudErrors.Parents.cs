namespace Estud.Back.Errors;

public class InvalidParentStudentsList : EstudError
{
    public static readonly InvalidParentStudentsList I = new();
    public override string Code { get; set; } = nameof(InvalidParentStudentsList);
    public override string Message { get; set; } = "Lista de alunos vinculados inválida.";
}

public class InvalidParentRelationship : EstudError
{
    public static readonly InvalidParentRelationship I = new();
    public override string Code { get; set; } = nameof(InvalidParentRelationship);
    public override string Message { get; set; } = "Parentesco inválido.";
}
