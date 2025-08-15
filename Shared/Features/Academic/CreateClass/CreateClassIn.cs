namespace Syki.Shared;

public class CreateClassIn
{
    public Guid DisciplineId { get; set; }
    public Guid? CampusId { get; set; }
    public Guid? TeacherId { get; set; }
    public string Period { get; set; }
    public int Vacancies { get; set; }
    public List<ScheduleIn> Schedules { get; set; }

    public CreateClassIn() { }

    public CreateClassIn(
        Guid disciplineId,
        Guid? campusId,
        Guid? teacherId,
        string period,
        int vacancies,
        List<ScheduleIn> schedules
    ) {
        DisciplineId = disciplineId;
        CampusId = campusId;
        TeacherId = teacherId;
        Period = period;
        Vacancies = vacancies;
        Schedules = schedules;
    }

    public static IEnumerable<(string, CreateClassIn)> GetExamples() =>
    [
        ("Banco de Dados",
        new CreateClassIn(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            "2024.1",
            40,
            [
                new(Day.Monday, Hour.H07_00, Hour.H10_00),
                new(Day.Thursday, Hour.H08_00, Hour.H10_30),
            ]
        )),
        ("Programação Orientada a Objetos",
        new CreateClassIn(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            "2024.2",
            40,
            [
                new(Day.Tuesday, Hour.H19_15, Hour.H22_00),
            ]
        )),
    ];
}
