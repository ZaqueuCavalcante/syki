namespace Syki.Shared;

public class CreateClassroomOut : IApiDto<CreateClassroomOut>
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public static IEnumerable<(string, CreateClassroomOut)> GetExamples() =>
    [
        ("Sala 05",
        new CreateClassroomOut
        {
            Id = Guid.CreateVersion7(),
            Name = "Sala 05",
        }),
        ("Laboratório de Química",
        new CreateClassroomOut
        {
            Id = Guid.CreateVersion7(),
            Name = "Laboratório de Química",
        }),
    ];

    public static implicit operator CreateClassroomOut(OneOf<CreateClassroomOut, ErrorOut> value)
    {
        return value.Success;
    }
}
