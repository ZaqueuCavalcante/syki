namespace Estud.Back.Features.Classrooms.UpdateClassroom;

public class UpdateClassroomIn : IApiDto<UpdateClassroomIn>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }

    public static IEnumerable<(string, UpdateClassroomIn)> GetExamples() =>
    [
        ("Sala 05",
        new UpdateClassroomIn
        {
            Id = 1,
            Name = "Sala 05",
            Capacity = 40,
        }),
        ("Laboratório de Química",
        new UpdateClassroomIn
        {
            Id = 2,
            Name = "Laboratório de Química",
            Capacity = 35,
        }),
    ];
}
