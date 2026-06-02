namespace Syki.Back.Errors;

public class CourseNotFound : SykiError
{
    public static readonly CourseNotFound I = new();
    public override string Code { get; set; } = nameof(CourseNotFound);
    public override string Message { get; set; } = "Curso não encontrado.";
}

public class InvalidCourseSession : SykiError
{
    public static readonly InvalidCourseSession I = new();
    public override string Code { get; set; } = nameof(InvalidCourseSession);
    public override string Message { get; set; } = "Turno inválido.";
}

public class InvalidCourseType : SykiError
{
    public static readonly InvalidCourseType I = new();
    public override string Code { get; set; } = nameof(InvalidCourseType);
    public override string Message { get; set; } = "Tipo de curso inválido.";
}

public class InvalidCourseName : SykiError
{
    public static readonly InvalidCourseName I = new();
    public override string Code { get; set; } = nameof(InvalidCourseName);
    public override string Message { get; set; } = "Nome de curso inválido.";
}

public class InvalidCoursesList : SykiError
{
    public static readonly InvalidCoursesList I = new();
    public override string Code { get; set; } = nameof(InvalidCoursesList);
    public override string Message { get; set; } = "Lista de cursos inválida.";
}
