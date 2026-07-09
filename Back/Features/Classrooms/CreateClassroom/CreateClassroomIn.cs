namespace Estud.Back.Features.Classrooms.CreateClassroom;

public class CreateClassroomIn : IApiDto<CreateClassroomIn>
{
    public int CampusId { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }

    public static IEnumerable<(string, CreateClassroomIn)> GetExamples() =>
    [
        ("Sala 05",
        new CreateClassroomIn
        {
            Name = "Sala 05",
            Capacity = 40,
            CampusId = 1,
        }),
        ("Laboratório de Química",
        new CreateClassroomIn
        {
            Name = "Laboratório de Química",
            Capacity = 35,
            CampusId = 2,
        }),
    ];
}
