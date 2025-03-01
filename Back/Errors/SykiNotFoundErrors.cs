namespace Syki.Back.Errors;

public class CourseOfferingNotFound : SykiError
{
    public override string Code { get; set; } = nameof(CourseOfferingNotFound);
    public override string Message { get; set; } = "Oferta de curso não encontrada.";
}
public class CourseNotFound : SykiError
{
    public override string Code { get; set; } = nameof(CourseNotFound);
    public override string Message { get; set; } = "Curso não encontrado.";
}
public class DisciplineNotFound : SykiError
{
    public override string Code { get; set; } = nameof(DisciplineNotFound);
    public override string Message { get; set; } = "Disciplina não encontrada.";
}
public class ClassNotFound : SykiError
{
    public override string Code { get; set; } = nameof(ClassNotFound);
    public override string Message { get; set; } = "Turma não encontrada.";
}
public class AcademicPeriodNotFound : SykiError
{
    public override string Code { get; set; } = nameof(AcademicPeriodNotFound);
    public override string Message { get; set; } = "Período acadêmico não encontrado.";
}
public class CampusNotFound : SykiError
{
    public override string Code { get; set; } = nameof(CampusNotFound);
    public override string Message { get; set; } = "Campus não encontrado.";
}
public class CourseCurriculumNotFound : SykiError
{
    public override string Code { get; set; } = nameof(CourseCurriculumNotFound);
    public override string Message { get; set; } = "Grade curricular não encontrada.";
}
public class InstitutionNotFound : SykiError
{
    public override string Code { get; set; } = nameof(InstitutionNotFound);
    public override string Message { get; set; } = "Instituição não encontrada.";
}
public class TeacherNotFound : SykiError
{
    public override string Code { get; set; } = nameof(TeacherNotFound);
    public override string Message { get; set; } = "Professor não encontrado.";
}
public class UserNotFound : SykiError
{
    public override string Code { get; set; } = nameof(UserNotFound);
    public override string Message { get; set; } = "Usuário não encontrado.";
}
public class LessonNotFound : SykiError
{
    public override string Code { get; set; } = nameof(LessonNotFound);
    public override string Message { get; set; } = "Aula não encontrada.";
}
public class EnrollmentPeriodNotFound : SykiError
{
    public override string Code { get; set; } = nameof(EnrollmentPeriodNotFound);
    public override string Message { get; set; } = "Período de matrícula não encontrado.";
}
public class ExamGradeNotFound : SykiError
{
    public override string Code { get; set; } = nameof(ExamGradeNotFound);
    public override string Message { get; set; } = "Prova não encontrada.";
}
public class CommandNotFound : SykiError
{
    public override string Code { get; set; } = nameof(CommandNotFound);
    public override string Message { get; set; } = "Comando não encontrado.";
}
