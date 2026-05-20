namespace Syki.Back.Errors;

public class InvalidDisciplineName : SykiError
{
    public static readonly InvalidDisciplineName I = new();
    public override string Code { get; set; } = nameof(InvalidDisciplineName);
    public override string Message { get; set; } = "Nome de disciplina inválido.";
}
