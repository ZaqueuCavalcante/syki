namespace Syki.Back.Shared;

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
