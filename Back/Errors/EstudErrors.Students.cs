namespace Estud.Back.Errors;

public class StudentNotFound : EstudError
{
    public static readonly StudentNotFound I = new();
    public override string Code { get; set; } = nameof(StudentNotFound);
    public override string Message { get; set; } = "Aluno não encontrado.";
}

public class CourseOfferingNotFound : EstudError
{
    public static readonly CourseOfferingNotFound I = new();
    public override string Code { get; set; } = nameof(CourseOfferingNotFound);
    public override string Message { get; set; } = "Oferta de curso não encontrada.";
}

public class StudentAlreadyEnrolledInCourseOffering : EstudError
{
    public static readonly StudentAlreadyEnrolledInCourseOffering I = new();
    public override string Code { get; set; } = nameof(StudentAlreadyEnrolledInCourseOffering);
    public override string Message { get; set; } = "Aluno já matriculado nesta oferta de curso.";
}

public class StudentAlreadyEnrolledInClass : EstudError
{
    public static readonly StudentAlreadyEnrolledInClass I = new();
    public override string Code { get; set; } = nameof(StudentAlreadyEnrolledInClass);
    public override string Message { get; set; } = "Aluno já matriculado nesta turma.";
}

public class StudentNotEnrolledInClass : EstudError
{
    public static readonly StudentNotEnrolledInClass I = new();
    public override string Code { get; set; } = nameof(StudentNotEnrolledInClass);
    public override string Message { get; set; } = "Aluno não matriculado nesta turma.";
}
