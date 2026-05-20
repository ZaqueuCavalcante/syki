namespace Syki.Back.Features.Courses.CreateCourseCurriculum;

public class CreateCourseCurriculumIn : IApiDto<CreateCourseCurriculumIn>
{
    public string Name { get; set; }
    public int CourseId { get; set; }
    public List<CreateCourseCurriculumDisciplineIn> Disciplines { get; set; } = [];

    public static IEnumerable<(string, CreateCourseCurriculumIn)> GetExamples() =>
    [
        (
            "Grade ADS",
            new()
            {
                Name = "Grade ADS",
                CourseId = 1,
                Disciplines = [new(1, 1, 55, 70)]
            }
        ),
    ];
}

public class CreateCourseCurriculumDisciplineIn
{
    public int Id { get; set; }
    public byte Period { get; set; }
    public byte Credits { get; set; }
    public ushort Workload { get; set; }

    public CreateCourseCurriculumDisciplineIn() { }

    public CreateCourseCurriculumDisciplineIn(
        int disciplineId,
        byte period,
        byte credits,
        ushort cargaHoraria
    ) {
        Id = disciplineId;
        Period = period;
        Credits = credits;
        Workload = cargaHoraria;
    }
}
