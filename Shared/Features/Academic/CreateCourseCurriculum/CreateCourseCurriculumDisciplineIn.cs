namespace Syki.Shared;

public class CreateCourseCurriculumDisciplineIn
{
    public Guid Id { get; set; }
    public byte Period { get; set; }
    public byte Credits { get; set; }
    public ushort Workload { get; set; }

    public CreateCourseCurriculumDisciplineIn() { }

    public CreateCourseCurriculumDisciplineIn(
        Guid disciplineId,
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
