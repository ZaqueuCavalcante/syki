namespace Syki.Back.Errors;

public class InvalidCourseCurriculumName : SykiError
{
    public static readonly InvalidCourseCurriculumName I = new();
    public override string Code { get; set; } = nameof(InvalidCourseCurriculumName);
    public override string Message { get; set; } = "Nome de grade curricular inválido.";
}
