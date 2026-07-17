namespace Estud.Back.Features.CourseCurriculums.UpdateCourseCurriculum;

public class CourseCurriculumWithCourseOffering : EstudError
{
    public static readonly CourseCurriculumWithCourseOffering I = new();
    public override string Code { get; set; } = nameof(CourseCurriculumWithCourseOffering);
    public override string Message { get; set; } = "Não é possível atualizar o currículo com ofertas de disciplina associadas.";
}
