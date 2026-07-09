namespace Estud.Back.Domain.CourseCurriculums;

public class CourseCurriculumDiscipline
{
    public int CourseCurriculumId { get; set; }
    public int DisciplineId { get; set; }
    public byte Period { get; set; }
    public byte Credits { get; set; }
    public ushort Workload { get; set; }

    public CourseCurriculumDiscipline(
        int disciplineId,
        byte period,
        byte credits,
        ushort workload
    ) {
        DisciplineId = disciplineId;
        Period = period;
        Credits = credits;
        Workload = workload;
    }
}
