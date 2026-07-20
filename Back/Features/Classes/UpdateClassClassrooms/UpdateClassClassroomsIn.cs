namespace Estud.Back.Features.Classes.UpdateClassClassrooms;

public class UpdateClassClassroomsIn : IApiDto<UpdateClassClassroomsIn>
{
    public List<UpdateClassClassroomIn> Classrooms { get; set; } = [];

    public static IEnumerable<(string, UpdateClassClassroomsIn)> GetExamples() =>
    [
        ("Exemplo", new()
        {
            Classrooms =
            [
                new() { Day = Day.Monday, Start = Hour.H07_00, End = Hour.H10_00, ClassroomId = 5 },
                new() { Day = Day.Wednesday, Start = Hour.H07_00, End = Hour.H10_00, ClassroomId = 5 },
            ],
        }),
    ];
}

public class UpdateClassClassroomIn
{
    public Day Day { get; set; }
    public Hour Start { get; set; }
    public Hour End { get; set; }

    /// <summary>
    /// Sala que sedia este horário.
    /// O horário informado precisa existir na turma (mesmo dia, início e fim).
    /// </summary>
    public int ClassroomId { get; set; }
}
