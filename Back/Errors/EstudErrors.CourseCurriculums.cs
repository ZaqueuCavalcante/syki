namespace Estud.Back.Errors;

public class CourseCurriculumNotFound : EstudError
{
    public static readonly CourseCurriculumNotFound I = new();
    public override string Code { get; set; } = nameof(CourseCurriculumNotFound);
    public override string Message { get; set; } = "Grade curricular não encontrada.";
}

public class InvalidCourseCurriculumName : EstudError
{
    public static readonly InvalidCourseCurriculumName I = new();
    public override string Code { get; set; } = nameof(InvalidCourseCurriculumName);
    public override string Message { get; set; } = "Nome de grade curricular inválido.";
}
