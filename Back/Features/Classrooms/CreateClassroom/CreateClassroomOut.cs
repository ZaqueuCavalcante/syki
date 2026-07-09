namespace Estud.Back.Features.Classrooms.CreateClassroom;

public class CreateClassroomOut : IApiDto<CreateClassroomOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateClassroomOut)> GetExamples() =>
    [
        ("Banco de Dados", new CreateClassroomOut { Id = 1 }),
    ];
}
