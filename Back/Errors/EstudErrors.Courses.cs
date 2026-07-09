namespace Estud.Back.Errors;

public class CourseNotFound : EstudError
{
    public static readonly CourseNotFound I = new();
    public override string Code { get; set; } = nameof(CourseNotFound);
    public override string Message { get; set; } = "Curso não encontrado.";
}

public class InvalidCourseSession : EstudError
{
    public static readonly InvalidCourseSession I = new();
    public override string Code { get; set; } = nameof(InvalidCourseSession);
    public override string Message { get; set; } = "Turno inválido.";
}

public class InvalidCourseType : EstudError
{
    public static readonly InvalidCourseType I = new();
    public override string Code { get; set; } = nameof(InvalidCourseType);
    public override string Message { get; set; } = "Tipo de curso inválido.";
}

public class InvalidCourseName : EstudError
{
    public static readonly InvalidCourseName I = new();
    public override string Code { get; set; } = nameof(InvalidCourseName);
    public override string Message { get; set; } = "Nome de curso inválido.";
}

public class InvalidCoursesList : EstudError
{
    public static readonly InvalidCoursesList I = new();
    public override string Code { get; set; } = nameof(InvalidCoursesList);
    public override string Message { get; set; } = "Lista de cursos inválida.";
}
