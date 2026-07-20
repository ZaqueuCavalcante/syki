namespace Estud.Back.Errors;

public class ParentNotFound : EstudError
{
    public static readonly ParentNotFound I = new();
    public override string Code { get; set; } = nameof(ParentNotFound);
    public override string Message { get; set; } = "Responsável não encontrado.";
}

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
