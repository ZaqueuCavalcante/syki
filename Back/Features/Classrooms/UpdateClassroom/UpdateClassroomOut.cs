namespace Estud.Back.Features.Classrooms.UpdateClassroom;

public class UpdateClassroomOut : IApiDto<UpdateClassroomOut>
{
    public int Id { get; set; }

    /// <summary>
    /// Nome
    /// </summary>
    public string Name { get; set; }

    public int CampusId { get; set; }

    /// <summary>
    /// Capacidade total de alunos
    /// </summary>
    public int Capacity { get; set; }

    public static IEnumerable<(string, UpdateClassroomOut)> GetExamples() =>
    [
        ("Sala 05",
        new UpdateClassroomOut
        {
            Id = 1,
            Name = "Sala 05",
            CampusId = 1,
            Capacity = 40,
        }),
    ];
}
