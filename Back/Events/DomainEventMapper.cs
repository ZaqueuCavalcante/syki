using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Cross.CreateInstitution;
using Syki.Back.Features.Teacher.AddExamGradeNote;
using Syki.Back.Features.Cross.SendResetPasswordToken;
using Syki.Back.Features.Cross.CreatePendingUserRegister;

namespace Syki.Back.Events;

public static class DomainEventMapper
{
    public static string ToPortugueseEventName(this string value)
    {
        if (value.IsEmpty()) return value;

        if (value.Contains(nameof(PendingUserRegisterCreatedDomainEvent))) return "Registro de usuário criado";
        if (value.Contains(nameof(StudentCreatedDomainEvent))) return "Aluno criado";
        if (value.Contains(nameof(TeacherCreatedDomainEvent))) return "Professor criado";
        if (value.Contains(nameof(InstitutionCreatedDomainEvent))) return "Instituição criada";
        if (value.Contains(nameof(ResetPasswordTokenCreatedDomainEvent))) return "Token de redefinição de senha gerado";
        if (value.Contains(nameof(ExamGradeNoteAddedDomainEvent))) return "Nota adicionada";

        return value;
    }
}
