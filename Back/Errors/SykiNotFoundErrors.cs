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
public class ClassActivityNoteNotFound : SykiError
{
    public override string Code { get; set; } = nameof(ClassActivityNoteNotFound);
    public override string Message { get; set; } = "Avaliação não encontrada.";
}
public class CommandNotFound : SykiError
{
    public override string Code { get; set; } = nameof(CommandNotFound);
    public override string Message { get; set; } = "Comando não encontrado.";
}
public class ClassActivityNotFound : SykiError
{
    public override string Code { get; set; } = nameof(ClassActivityNotFound);
    public override string Message { get; set; } = "Atividade não encontrada.";
}
public class WebhookNotFound : SykiError
{
    public override string Code { get; set; } = nameof(WebhookNotFound);
    public override string Message { get; set; } = "Webhook não encontrado.";
}
public class WebhookCallNotFound : SykiError
{
    public override string Code { get; set; } = nameof(WebhookCallNotFound);
    public override string Message { get; set; } = "Chamada de webhook não encontrada.";
}

public class CurrentAcademicPeriodNotFound : SykiError
{
    public override string Code { get; set; } = nameof(CurrentAcademicPeriodNotFound);
    public override string Message { get; set; } = "Período acadêmico atual não encontrado.";
}
public class ClassroomNotFound : SykiError
{
    public override string Code { get; set; } = nameof(ClassroomNotFound);
    public override string Message { get; set; } = "Sala não encontrada.";
}
