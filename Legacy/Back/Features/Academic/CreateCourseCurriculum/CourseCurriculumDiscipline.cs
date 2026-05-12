namespace Syki.Back.Features.Academic.CreateCourseCurriculum;

public class CourseCurriculumDiscipline
{
    public Guid CourseCurriculumId { get; set; }
    public Guid DisciplineId { get; set; }
    public byte Period { get; set; }
    public byte Credits { get; set; }
    public ushort Workload { get; set; }
    public List<Guid> PreRequisites { get; set; }

    public CourseCurriculumDiscipline(
        Guid disciplineId,
        byte period,
        byte credits,
        ushort workload
    ) {
        DisciplineId = disciplineId;
        Period = period;
        Credits = credits;
        Workload = workload;
    }

    public void AddPreRequisites(List<Guid> preRequisites)
    {
        PreRequisites = preRequisites;
    }
}
