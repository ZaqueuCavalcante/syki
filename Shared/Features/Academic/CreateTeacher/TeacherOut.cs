namespace Syki.Shared;

public class TeacherOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public static implicit operator TeacherOut(OneOf<TeacherOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
