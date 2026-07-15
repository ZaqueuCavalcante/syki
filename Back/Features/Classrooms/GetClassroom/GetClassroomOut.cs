namespace Estud.Back.Features.Classrooms.GetClassroom;

public class GetClassroomOut : IApiDto<GetClassroomOut>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CampusId { get; set; }
    public string Campus { get; set; }
    public int Capacity { get; set; }

    /// <summary>
    /// Agenda da sala: turmas alocadas com seus dias e horários.
    /// </summary>
    public List<ClassroomScheduleOut> Schedules { get; set; } = [];

    public static IEnumerable<(string, GetClassroomOut)> GetExamples() =>
    [
        ("Exemplo", new GetClassroomOut
        {
            Id = 1,
            Name = "Sala 05",
            CampusId = 1,
            Campus = "Campus Agreste",
            Capacity = 40,
            Schedules =
            [
                new ClassroomScheduleOut
                {
                    ClassId = 1,
                    Discipline = "Banco de Dados",
                    Teacher = "Chico Ferreira",
                    Day = Day.Monday,
                    StartAt = Hour.H07_00,
                    EndAt = Hour.H10_00,
                },
            ],
        }),
    ];
}

public class ClassroomScheduleOut
{
    public int ClassId { get; set; }
    public string Discipline { get; set; }
    public string Teacher { get; set; }
    public Day Day { get; set; }
    public Hour StartAt { get; set; }
    public Hour EndAt { get; set; }
}
